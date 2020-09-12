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
                .UseWindowsService()    //按照Windows Service运行
                .ConfigureServices((hostContext, services) =>
                {
                    //注册服务
                    services.AddMyServices(hostContext.Configuration)
                            .AddHostedService<Worker>();
                });
    }
}