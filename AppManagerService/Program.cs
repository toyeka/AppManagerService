using System;

using log4net;
using log4net.Config;

using Topshelf;

namespace AppManagerService
{
    public class Program
    {
        private static ILog logger;

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            logger = LogManager.GetLogger(typeof(Program));
            logger.Info("Logger initialized.");

            HostFactory.Run(x =>
            {
                x.Service<TownCrier>(s =>
                {
                    s.ConstructUsing(name => new TownCrier(logger));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Sample Topshelf Host");
                x.SetDisplayName("Stuff");
                x.SetServiceName("Stuff");
            });

            Console.ReadKey();
        }
    }
}