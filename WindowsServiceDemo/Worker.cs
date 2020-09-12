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
            _logger.LogInformation("��������");

            //����һ��������
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler(stoppingToken);

            //ָ���Զ����JobFactory
            scheduler.JobFactory = _jobFactory;

            //����Job
            var sendMsgJob = JobBuilder.Create<SendMsgJob>()
                .WithIdentity(nameof(SendMsgJob), nameof(Worker))
                .Build();
            //����������
            var sendMsgTrigger = TriggerBuilder.Create()
                .WithIdentity("trigger-" + nameof(SendMsgJob), "trigger-group-" + nameof(Worker))
                .StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(08, 30)) //ÿ�յ�08:30ִ��
                .Build();

            await scheduler.Start(stoppingToken);
            //��Job�ʹ����������������
            await scheduler.ScheduleJob(sendMsgJob, sendMsgTrigger, stoppingToken);
        }
    }
}
