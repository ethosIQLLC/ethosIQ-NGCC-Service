using ethosIQ_Configuration;
using ethosIQ_Database;
using ethosIQ_NGCC_Shared;
using ethosIQ_NGCC_Shared.DAO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ethosIQ_NGCC_Service
{
    public class NGCCSourceController : ethosIQController
    {
        public NGCCSourceController(string Name, string LogName) : base(Name, LogName)
        {
            /*
            try
            {
                if (!EventLog.SourceExists("NGCC Source Controller"))
                {
                    EventLog.CreateEventSource("NGCC Source Controller", "NGCC-Service");
                }

                EventLog = new EventLog();

                EventLog.Log = "NGCC-Service";
                EventLog.Source = "NGCC Source Controller";
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to setup event viewer logging. " + exception.Message);
            }
            */
        }

        /*
        public void StartSources()
        {
            
            try
            {
                ConfigurationDatabase = LocalConfigurationDatabase.GetConfigurationDatabase();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to get configuration database. " + exception.Message);

                if (EventLog != null)
                {
                    EventLog.WriteEntry("Failed to get configuration database. " + exception.Message, EventLogEntryType.Error);
                }
            }

            try
            {
                if (ConfigurationDatabase != null)
                {
                    CollectionDatabaseConfiguration collectionDatabaseConfiguration = new CollectionDatabaseConfiguration(ConfigurationDatabase);
                    CollectionDatabase = collectionDatabaseConfiguration.GetCollectionDatabase();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to get collection database. " + exception.Message);

                if (EventLog != null)
                {
                    EventLog.WriteEntry("Failed to get collection database. " + exception.Message, EventLogEntryType.Error);
                }
            }

            try
            {
                NGCCSources = GetFileSources();

                
                if (NGCCSources != null)
                {
                    foreach (NGCCSource ngccSource in NGCCSources)
                    {
                        ngccSource.AddCollectionDatabase(CollectionDatabase);
                        ngccSource.AddConfigurationDatabase(ConfigurationDatabase);
                        ngccSource.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to get NGCC source information from configuration database. " + exception.Message);

                if (EventLog != null)
                {
                    EventLog.WriteEntry("Failed to get NGCC source information from configuration database. " + exception.Message, EventLogEntryType.Error);
                }
            }
            
        }

        public void StopSources()
        {
            
            try
            {
                if (NGCCSources != null)
                {
                    foreach (NGCCSource ngccSource in NGCCSources)
                    {
                        ngccSource.Stop();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to stop service. " + exception.Message);

                if (EventLog != null)
                {
                    EventLog.WriteEntry("Failed to stop service. " + exception.Message, EventLogEntryType.Error);
                }
            }
            
        }
        */

        public override bool GetSources()
        {

            if (ConfigurationDatabase != null)
            {
                Sources = new List<ethosIQSource>();

                NGCCSourceDAO ngccSourceDAO = new NGCCSourceDAO(ConfigurationDatabase);

                foreach(NGCCSource source in ngccSourceDAO.GetAllSources())
                {
                    Sources.Add(source);
                }
                
                return true;
            }

            return false;
        }
    }
}
