using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServiceDemo.Interceptor
{
    public class AuthenticRequestDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// 增加额外的Header信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            request.Headers.Add("time", now.ToString("yyyyMMddHHms"));
            request.Headers.Add("token", "hasaki");
            var result = await base.SendAsync(request, cancellationToken);
            return result;
        }
    }
}