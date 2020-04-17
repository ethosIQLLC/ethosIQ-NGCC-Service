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
        public string RealTimeURL;

        public Timer GetRealTime;
        public Timer RealTimeCheck;
        public WFMRealTimeClient RealTimeClient;
        public object RealTimeLock;

        public List<Translation> Translations;
        public List<AgentState> CurrentStates;
        public List<AgentState> LastStates;

        public NGCCSource(int ID, string Name, string IPAddress, int Port, string TenantID, string Username, string Password, bool RealTimeEnabled, string RealTimeURL)
        {
            this.ID = ID;
            this.Name = Name;
            this.IPAddress = IPAddress;
            this.Port = Port;
            this.TenantID = TenantID;
            this.Username = Username;
            this.Password = Password;
            this.RealTimeURL = RealTimeURL;

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
                BasicHttpBinding Binding = new BasicHttpBinding();
                RealTimeClient = new WFMRealTimeClient(Binding, new EndpointAddress(RealTimeURL));

                RealTimeCheck = new Timer(30000);
                RealTimeCheck.Elapsed += new ElapsedEventHandler(OnRealTimeCheck);
                RealTimeCheck.Start();
            }
            catch(Exception exception)
            {
                if(EventLog != null)
                {
                    EventLog.WriteEntry("Failed to start timer to check real time endpoint status. " + exception.Message, EventLogEntryType.Error);
                }
            }

            try
            {
                GetRealTime = new Timer(10000);
                GetRealTime.Elapsed += GetRealTimeAgentState;
                GetRealTime.Start();
            }
            catch(Exception exception)
            {
                if (EventLog != null)
                {
                    EventLog.WriteEntry("Failed to start timer to retrieve real time states. " + exception.Message, EventLogEntryType.Error);
                }
            }

            try
            {
                foreach(string line in File.ReadAllLines(Environment.CurrentDirectory + @"\translate.txt"))
                {
                    string[] agent = line.Split('=');

                    Translations.Add(new Translation(agent[0], agent[1]));
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(Environment.CurrentDirectory);
                Console.WriteLine("Translations: " + exception.Message);
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
                WebClient webClient = new WebClient();
                webClient.Credentials = new NetworkCredential(Username, Password);
                XML = webClient.DownloadString("https://" + IPAddress + ":" + Port + "/rtrdll/rtrweb.dll?Tenant=" + TenantID + "&Filter=ACalls");
            }
            catch(Exception exception)
            {
                if(EventLog != null)
                {
                    EventLog.WriteEntry("Failed to get XML file containing real time states. " + exception.Message, EventLogEntryType.Error);
                }

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
                        foreach (AgentActiveCall call in calls.A)
                        {
                            CurrentStates.Add(new AgentState(call.Id, call.As));

                            Translation translation = Translations.Where(x => x.PrimaryID == call.Id).FirstOrDefault();
                            
                            if (call.Rc != null)
                            {
                                RealTimeClient.SendAgentState(translation.SecondaryID, call.As + "_" + call.Rc, DateTime.Now.AddSeconds(-Convert.ToInt32(call.Tp)));
                            }
                            else
                            {
                                RealTimeClient.SendAgentState(translation.SecondaryID, call.As, DateTime.Now.AddSeconds(-Convert.ToInt32(call.Tp)));
                            }
                        }

                        foreach (AgentState lastState in LastStates)
                        {
                            AgentState searchingLastState = CurrentStates.Where(x => x.Id == lastState.Id).FirstOrDefault();

                            if(searchingLastState == null)
                            {
                                Translation translation = Translations.Where(x => x.PrimaryID == lastState.Id).FirstOrDefault();

                                RealTimeClient.SendAgentState(translation.SecondaryID, "0", DateTime.Now);
                            }
                        }

                        LastStates = CurrentStates;
                        CurrentStates = new List<AgentState>();
                    }
                }
                catch (Exception exception)
                {
                    if (EventLog != null)
                    {
                        EventLog.WriteEntry("Failed to serialize XML file containing real time states. " + exception.Message, EventLogEntryType.Error);
                    }

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
                    RealTimeClient.SendAgentState("ethosIQ", "Heartbeat", DateTime.Now);
                    RealTimeEnabled = true;
                }
                catch (Exception exception)
                {
                    RealTimeEnabled = false;

                    if (EventLog != null)
                    {
                        EventLog.WriteEntry("Failed to make a connection to the real time service. Temporarily disabling. " + exception.Message, EventLogEntryType.Error);
                    }

                    Console.WriteLine("Failed to make a connection to the real time service. Temporarily disabling. " + exception.Message);
                }
            }
        }
    }
}
