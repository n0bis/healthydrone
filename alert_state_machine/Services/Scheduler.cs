using System;


namespace alert_state_machine.Services
{
    public static class Scheduler
    {
        public static void IntervalInMinutes(double interval, Action task)
        {
            interval = interval / 60;
            SchedulerService.Instance.ScheduleTask(0,0,interval,task);
        }
        public static void IntervalInHours(double interval, Action task)
        {
            SchedulerService.Instance.ScheduleTask(0,0,interval,task);
        }
        public static void IntervalInDays(int hour, int min, double interval, Action task)
        {
            interval = interval * 24;
            SchedulerService.Instance.ScheduleTask(hour,min,interval,task);
        }
    }
}