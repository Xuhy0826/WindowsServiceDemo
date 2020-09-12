using System.Threading;
using System.Threading.Tasks;
using WindowsServiceDemo.Jobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace WindowsServiceDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MyJobFactory _jobFactory;

        public Worker(ILogger<Worker> logger, MyJobFactory jobFactory)
        {
            _logger = logger;
            _jobFactory = jobFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("服务启动");

            //创建一个调度器
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler(stoppingToken);

            //指定自定义的JobFactory
            scheduler.JobFactory = _jobFactory;

            //创建Job
            var sendMsgJob = JobBuilder.Create<SendMsgJob>()
                .WithIdentity(nameof(SendMsgJob), nameof(Worker))
                .Build();
            //创建触发器
            var sendMsgTrigger = TriggerBuilder.Create()
                .WithIdentity("trigger-" + nameof(SendMsgJob), "trigger-group-" + nameof(Worker))
                .StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(08, 30)) //每日的08:30执行
                .Build();

            await scheduler.Start(stoppingToken);
            //把Job和触发器放入调度器中
            await scheduler.ScheduleJob(sendMsgJob, sendMsgTrigger, stoppingToken);
        }
    }
}
