using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace ethosIQ_NGCC_Service
{
    [System.ServiceModel.ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.Single)]
    public class Program : ServiceBase
    {
        public NGCCSourceController controller;

        public static void Main(string[] args)
        {
            ServiceHost serviceHost;
            Program service = new Program();
            if (Environment.UserInteractive)
            {

                serviceHost = new ServiceHost(service);
                serviceHost.Open();
                service.OnStart(args);
                Console.ReadLine();
            }
            else
            {
                serviceHost = new ServiceHost(service);
                serviceHost.Open();
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                controller = new NGCCSourceController("NGCC Controller", "NGCC-Service");
                controller.Initialize();
                controller.StartSources();

                Console.WriteLine("ethosIQ NGCC Service started!");
            }
            catch(Exception exception)
            {
                Console.WriteLine("Failed to start service. " + exception.Message);
            }
        }

        protected override void OnStop()
        {
            controller.StopSources();
        }
    }
}
