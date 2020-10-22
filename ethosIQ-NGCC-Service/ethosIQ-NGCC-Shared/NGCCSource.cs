using System;
using System.Timers;
using System.Net;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using ethosIQ_Configuration;
using ethosIQ_NGCC_Shared.WFMRealTime;
using System.ServiceModel;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using ethosIQ_RTA;
using ethosIQ_NGCC_Shared.DAO;

namespace ethosIQ_NGCC_Shared
{
    public class NGCCSource : ethosIQSource
    {
        public string IPAddress;
        public int Port;
        public string TenantID;
        public string Username;
        public string Password;
        public bool RealTimeEnabled;
        public string RealTimeIPAddress;
        public int RealTimePort;

        public Timer GetRealTime;
        public Timer RealTimeCheck;
        public WFMRealTimeSource RealTimeClient;
        public object RealTimeLock;

        public List<Translation> Translations;
        public List<AgentState> CurrentStates;
        public List<AgentState> LastStates;

        public NGCCSource(string Name, string IPAddress, int Port, string TenantID, string Username, string Password, bool RealTimeEnabled, string RealTimeIPAddress, int RealTimePort)
        {
            this.Name = Name;
            this.IPAddress = IPAddress;
            this.Port = Port;
            this.TenantID = TenantID;
            this.Username = Username;
            this.Password = Password;
            this.RealTimeIPAddress = RealTimeIPAddress;
            this.RealTimePort = RealTimePort;

            RealTimeLock = new object();
            Translations = new List<Translation>();
            CurrentStates = new List<AgentState>();
            LastStates = new List<AgentState>();
        }

        public NGCCSource(int ID, string Name, string IPAddress, int Port, string TenantID, string Username, string Password, bool RealTimeEnabled, string RealTimeIPAddress, int RealTimePort)
        {
            this.ID = ID;
            this.Name = Name;
            this.IPAddress = IPAddress;
            this.Port = Port;
            this.TenantID = TenantID;
            this.Username = Username;
            this.Password = Password;
            this.RealTimeIPAddress = RealTimeIPAddress;
            this.RealTimePort = RealTimePort;

            RealTimeLock = new object();
            Translations = new List<Translation>();
            CurrentStates = new List<AgentState>();
            LastStates = new List<AgentState>();
        }

        public override void Start()
        {
            InitializeEventLog("NGCC-Service");

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                RealTimeClient = new WFMRealTimeSource(RealTimeIPAddress, RealTimePort, 30000);
                RealTimeClient.Start();
                LogInformation("Successfully started real time client.");
            }
            catch(Exception exception)
            {
                LogError("Failed to start real time client. " + exception.Message);
            }

            try
            {
                RealTimeCheck = new Timer(10000);
                RealTimeCheck.Elapsed += OnRealTimeCheck;
                RealTimeCheck.Start();

                LogInformation("Successfully started real time check timer.");
            }
            catch(Exception e)
            {

            }

            try
            {
                GetRealTime = new Timer(60000);
                GetRealTime.Elapsed += GetRealTimeAgentState;
                GetRealTime.Start();
                LogInformation("Successfully started timer to retrieve real time states.");
            }
            catch(Exception exception)
            {
                LogError("Failed to start timer to retrieve real time states. " + exception.Message);
            }

            try
            {
                TranslationDAO translationDAO = new TranslationDAO(CollectionDatabase);
                Translations = translationDAO.GetTranslation();
                if (Translations != null)
                {
                    LogInformation("Successfully pulled " + Translations.Count + " translations for NGCC agents.");
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(Environment.CurrentDirectory);
                Console.WriteLine("Translations: " + exception.Message);
                LogError("Failed to pull translations for NGCC agents. " + exception.Message);
            }

        }

        public override void Stop()
        {

        }

        public override void Restart()
        {
            
        }

        private void GetRealTimeAgentState(object sender, ElapsedEventArgs e)
        {
            string XML = "";

            try
            {
                Console.WriteLine("Pulling...");
                WebClient webClient = new WebClient();
                webClient.Credentials = new NetworkCredential(Username, Password);
                XML = webClient.DownloadString("https://" + IPAddress + ":" + Port + "/rtrdll/rtrweb.dll?Tenant=" + TenantID + "&Filter=ACalls");
                Console.WriteLine("Completed...");
            }
            catch(Exception exception)
            {
                LogError("Failed to get XML file containing real time states. " + exception.Message);

                Console.WriteLine("Failed to get XML file containing real time states. " + exception.Message);
            }

            if (XML != "")
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ACalls));
                    StringReader stringReader = new StringReader(XML);
                    ACalls calls = (ACalls)serializer.Deserialize(stringReader);

                    //DateTime TimeStamp = DateTime.ParseExact(calls.Timestamp, "ddd MMM dd HH:mm:ss yyyy Z", CultureInfo.InvariantCulture);
                    if (RealTimeClient != null && RealTimeEnabled)
                    {
                        Console.WriteLine("Looping through " + calls.A.Count+ " Agents...");
                        foreach (AgentActiveCall call in calls.A)
                        {
                            CurrentStates.Add(new AgentState(call.Id, call.As));
                            Translation translation = Translations.Where(x => x.PrimaryID == call.Id).FirstOrDefault();

                            if (translation != null)
                            {
                                if (call.Rc != null)
                                {
                                    RealTimeClient.SendAgentState(translation.GetID(), call.As + "_" + call.Rc, DateTime.Now.AddSeconds(-Convert.ToInt32(call.Tp)));
                                    Console.WriteLine("Translation - ReasonCode - " + translation.GetID());
                                }
                                else
                                {
                                    RealTimeClient.SendAgentState(translation.GetID(), call.As, DateTime.Now.AddSeconds(-Convert.ToInt32(call.Tp)));
                                    Console.WriteLine("Translation - " + translation.GetID());
                                }
                            }
                            else
                            {
                                if (call.Rc != null)
                                {
                                    RealTimeClient.SendAgentState(call.Id, call.As + "_" + call.Rc, DateTime.Now.AddSeconds(-Convert.ToInt32(call.Tp)));
                                    Console.WriteLine("No Translation - ReasonCode - " + call.Id);
                                }
                                else
                                {
                                    RealTimeClient.SendAgentState(call.Id, call.As, DateTime.Now.AddSeconds(-Convert.ToInt32(call.Tp)));
                                    Console.WriteLine("No Translation - " + call.Id);
                                }
                            }
                        }
                        foreach (AgentState lastState in LastStates)
                        {
                            AgentState searchingLastState = CurrentStates.Where(x => x.Id == lastState.Id).FirstOrDefault();

                            if(searchingLastState == null)
                            {
                                Translation translation = Translations.Where(x => x.PrimaryID == lastState.Id).FirstOrDefault();

                                if (translation != null)
                                {
                                    RealTimeClient.SendAgentState(translation.SecondaryID, "0", DateTime.Now);
                                }
                            }
                        }

                        LastStates = CurrentStates;
                        CurrentStates = new List<AgentState>();
                    }

                    Console.WriteLine("Finished interval...");
                }
                catch (Exception exception)
                {
                    LogError("Failed to serialize XML file containing real time states. " + exception.Message + "\n " + exception.StackTrace);
                    Console.WriteLine("Failed to serialize XML file containing real time states. " + exception.Message);
                }
            }
        }

        private void OnRealTimeCheck(object sender, ElapsedEventArgs e)
        {
            lock (RealTimeLock)
            {
                try
                {
                    RealTimeClient.SendAgentState("ethosIQ", "NGCC-Heartbeat", DateTime.Now);
                    RealTimeEnabled = true;
                }
                catch (Exception exception)
                {
                    RealTimeEnabled = false;

                    LogWarning("Failed to make a connection to the real time service. Temporarily disabling. " + exception.Message);
                    
                    Console.WriteLine("Failed to make a connection to the real time service. Temporarily disabling. " + exception.Message);
                }
            }
        }
    }
}
