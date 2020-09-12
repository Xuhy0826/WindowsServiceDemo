using System;
using System.Collections.Generic;
using System.Text;
using WindowsServiceDemo.Interceptor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WindowsServiceDemo
{
    public static class DependencyInject
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration config)
        {
            //配置
            services.Configure<AppSettings>(config);
            //注册“命名HttpClient”，并为其配置拦截器
            services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(config["ApiBaseUrl"]);
            }).AddHttpMessageHandler(_ => new AuthenticRequestDelegatingHandler());


            return services;
        }
    }
}
