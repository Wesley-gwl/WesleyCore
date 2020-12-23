using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WesleyCore.Extensions.Http
{
    /// <summary>
    /// 客户端请求
    /// </summary>
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(requestUri, content, cancellationToken);
        }
    }
}