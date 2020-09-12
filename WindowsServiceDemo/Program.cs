using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WindowsServiceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()    //����Windows Service����
                .ConfigureServices((hostContext, services) =>
                {
                    //ע�����
                    services.AddMyServices(hostContext.Configuration)
                            .AddHostedService<Worker>();
                });
    }
}