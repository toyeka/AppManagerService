using System;

using AppManagerInterface;

namespace NewTesstApp
{
    public class TestApp : IScheduledTask
    {
        public string TimerCron { get; set; }

        //in app need to add interface and rename entrypoint method.
        public void StartPoint()
        {
            Console.WriteLine("YOU ARE Welcome! " + DateTime.Now);
//            throw new Exception();
        }

        public static void Main() {}
    }
}