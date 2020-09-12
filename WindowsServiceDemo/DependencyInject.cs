using System;
using WindowsServiceDemo.Interceptor;
using WindowsServiceDemo.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WindowsServiceDemo
{
    public static class DependencyInject
    {
        /// <summary>
        /// 定义扩展方法，注册服务
        /// </summary>
        public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration config)
        {
            //配置文件
            services.Configure<AppSettings>(config);

            //注册“命名HttpClient”，并为其配置拦截器
            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(config["ApiBaseUrl"]);
            }).AddHttpMessageHandler(_ => new AuthenticRequestDelegatingHandler());

            //注册任务
            services.AddSingleton<SendMsgJob>();

            //添加Job工厂
            services.AddSingleton<MyJobFactory>();

            return services;
        }
    }
}
