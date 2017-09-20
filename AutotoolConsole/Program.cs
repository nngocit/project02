using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using NLog;
using Quartz;
using Quartz.Impl;

namespace AutotoolConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Grab the Scheduler instance from the Factory 
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            // and start it off
            scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<CallAutoToolApiJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every <interval> seconds
            int interval = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Interval"));
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(interval)
                    .RepeatForever())
                .Build();

            Console.WriteLine($"{DateTime.Now} >> Start WebComment Autotool!");

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
            Thread.Sleep(TimeSpan.FromSeconds(60));

            // and last shut down the scheduler when you are ready to close your program
            //scheduler.Shutdown();
        }
    }

    public class CallAutoToolApiJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            int status = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Status"));
            if (status == 0)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug($"WebComment Autotool() - Autotool is OFF");
                Console.WriteLine($"{DateTime.Now} >> Autotool is OFF");
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            string WebCommentAPI = ConfigurationManager.AppSettings.Get("WebCommentAPI");
            var client = new RestClient(WebCommentAPI);
            var req = new RestRequest("/comments/autotool", Method.GET) { RequestFormat = DataFormat.Json };
            var response = client.Execute<dynamic>(req);
            Console.WriteLine($"{DateTime.Now} >> Requesting API: {WebCommentAPI}comments/autotool...");

            if (response != null && response.Data == "SUCCESS")
            {
                NLog.LogManager.GetCurrentClassLogger().Debug($"WebComment Autotool() - Run WebComment autotool responsed SUCCESS!");
                Console.WriteLine($"{DateTime.Now} >> Run WebComment autotool responsed SUCCESS!");
            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger()
                    .Debug(response != null
                        ? $"WebComment Autotool() - Run WebComment autotool responsed FAILED: {response.Data}"
                        : $"WebComment Autotool() - No response from server!");
                Console.WriteLine(response != null
                        ? $"{DateTime.Now} >> Run WebComment autotool responsed FAILED: {response.Data}"
                        : $"{DateTime.Now} >> No response from server!");
            }

            LogManager.GetCurrentClassLogger().Debug($"WebComment Autotool() - Finished autorun at {DateTime.Now}, total time {stopWatch.Elapsed.TotalSeconds}");
            Console.WriteLine($"{DateTime.Now} >> Finished autorun at {DateTime.Now}, total time {stopWatch.Elapsed.TotalSeconds}");
            stopWatch.Stop();

            //Console.ReadKey();
        }
    }
}
