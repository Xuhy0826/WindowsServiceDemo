using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using Quartz;

namespace WindowsServiceDemo
{
    public class MyJobFactory : IJobFactory
    {
        /// <summary>
        /// Job工厂，从服务容器中取Job
        /// </summary>
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
