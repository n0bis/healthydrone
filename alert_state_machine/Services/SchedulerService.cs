using System;
using System.Collections.Generic;
using System.Threading;


namespace alert_state_machine.Services
{
    public class SchedulerService
    {
        private static SchedulerService instance;
        private List<Timer> timers = new List<Timer>();

        private SchedulerService()
        {
            
        }

        public static SchedulerService Instance => instance ??= new SchedulerService();

        public void ScheduleTask(int hour, int min, double intervalInHours, Action task)
        {
            /*DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year,now.Month,now.Day,hour,min,0,0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }*/

            var timer = new Timer(x => { task.Invoke(); }, null, TimeSpan.Zero, TimeSpan.FromHours(intervalInHours));
            timers.Add(timer);
        }
        
    }
}