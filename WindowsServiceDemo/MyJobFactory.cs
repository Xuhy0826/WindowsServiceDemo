using Quartz.Spi;
using System;
using Quartz;

namespace WindowsServiceDemo
{
    /// <summary>
    /// Job工厂，从服务容器中取Job
    /// </summary>
    public class MyJobFactory : IJobFactory
    {
        protected readonly IServiceProvider _serviceProvider;
        public MyJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobType = bundle.JobDetail.JobType;
            try
            {
                var job = _serviceProvider.GetService(jobType) as IJob;
                return job;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
