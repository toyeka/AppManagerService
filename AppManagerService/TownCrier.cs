using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

using AppManagerInterface;

using Container;

using log4net;

using Microsoft.Practices.Unity;

using NCrontab;

namespace AppManagerService
{
    public class TownCrier
    {
        private const string TestSystem = "TEST_system";
        private readonly ILog logger;
        private readonly Emailer mailer;

        private readonly List<CronTaskSheduler> sheduler = new List<CronTaskSheduler>();
        private readonly Timer timer;

        public TownCrier(ILog logger)
        {
            this.logger = logger;

            var tenant = ContainerManager.Instance.Tenants.FirstOrDefault(t => t.TenantName == TestSystem);
            if (tenant == null)
            {
                Console.WriteLine($"Mandant '{TestSystem}' not found in Unity configuration");
                Console.ReadKey();
                return;
            }

            var childApps = tenant.UnityContainer.Resolve<IScheduledTask[]>();
            foreach (var childApp in childApps)
            {
                var crontab = CrontabSchedule.TryParse(childApp.TimerCron);
                //logger: if parameter empty - skip this job with error message
                if (crontab == null)
                {
                    logger.ErrorFormat(
                        "Parameter TimerCron for Library '{0}' not set. App from Library '{0}' not started. Please check Unity configuration file.",
                        childApp.GetType());
                }
                else
                {
                    sheduler.Add(new CronTaskSheduler(childApp, crontab, DateTime.MinValue));
                }
            }

            mailer = new Emailer(logger);

            timer = new Timer(5000) { AutoReset = true };
            timer.Elapsed += (sender, eventArgs) => DoWork();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void DoWork()
        {
            foreach (var task in sheduler)
            {
                var nextOccurrence = task.Crontab.GetNextOccurrence(task.LastStart);
                if (nextOccurrence < DateTime.Now)
                {
                    task.LastStart = DateTime.Now;
                    try
                    {
                        task.ScheduledTask.StartPoint();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        mailer.SendMail("Test mail", ex.ToString());
                    }
                }
            }
        }
    }
}