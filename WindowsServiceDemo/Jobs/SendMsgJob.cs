using System;
using System.Net.Http;
using System.Threading.Tasks;
using WindowsServiceDemo.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace WindowsServiceDemo.Jobs
{
    public class SendMsgJob : IJob
    {
        private readonly AppSettings _appSettings;
        private const string ApiClientName = "ApiClient";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SendMsgJob> _logger;

        public SendMsgJob(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<SendMsgJob> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// 定时执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"开始执行定时任务");
            try
            {
                //从httpClientFactory获取我们注册的named-HttpClient
                using var client = _httpClientFactory.CreateClient(ApiClientName);
                var message = new
                {
                    title = "今日消息",
                    content = _appSettings.MessageNeedToSend
                };
                //发送消息
                var response = await client.PostAsync("/msg", new JsonContent(message));
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"消息发送成功");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
