using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;

namespace TimeSheetCommon
{
    //public class EmailService
    //{
    //    int getCallType;
    //    Timer timer1;

    //    public EmailService()
    //    {
    //        ///InitializeComponent();
    //        int strTime = Convert.ToInt32(ConfigurationManager.AppSettings["callDuration"]);
    //        getCallType = Convert.ToInt32(ConfigurationManager.AppSettings["CallType"]);
    //        if (getCallType == 1)
    //        {
    //            timer1 = new System.Timers.Timer();
    //            double inter = (double)GetNextInterval();
    //            timer1.Interval = inter;
    //            timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
    //        }
    //        else
    //        {
    //            timer1 = new System.Timers.Timer();
    //            timer1.Interval = strTime * 1000;
    //            timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
    //        }
    //    }

    //    private double GetNextInterval()
    //    {
    //        timeString = ConfigurationManager.AppSettings["StartTime"];
    //        DateTime t = DateTime.Parse(timeString);
    //        TimeSpan ts = new TimeSpan();
    //        int x;
    //        ts = t - System.DateTime.Now;
    //        if (ts.TotalMilliseconds < 0)
    //        {
    //            ts = t.AddDays(1) - System.DateTime.Now;//Here you can increase the timer interval based on your requirments.   
    //        }
    //        return ts.TotalMilliseconds;
    //    }
    //    private void SetTimer()
    //    {
    //        try
    //        {
    //            double inter = (double)GetNextInterval();
    //            timer1.Interval = inter;
    //            timer1.Start();
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }
    //}
}