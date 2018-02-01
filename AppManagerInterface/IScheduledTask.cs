namespace AppManagerInterface
{
    public interface IScheduledTask
    {
        string TimerCron { get; set; }

        void StartPoint();

//        void StartPoint(string timer);
    }
}