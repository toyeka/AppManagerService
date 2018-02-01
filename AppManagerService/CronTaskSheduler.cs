using System;

using AppManagerInterface;

using NCrontab;

namespace AppManagerService
{
    public class CronTaskSheduler
    {
        public CronTaskSheduler(IScheduledTask appInst, CrontabSchedule cron, DateTime lastStart)
        {
            ScheduledTask = appInst;
            Crontab = cron;
            LastStart = lastStart;
        }

        public IScheduledTask ScheduledTask { get; set; }

        public CrontabSchedule Crontab { get; set; }

        public DateTime LastStart { get; set; }
    }
}