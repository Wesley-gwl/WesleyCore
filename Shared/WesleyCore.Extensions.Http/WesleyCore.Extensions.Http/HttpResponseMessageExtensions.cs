using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WesleyCore.Extensions.Http
{
    /// <summary>
    /// 返回参数json化
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        public async static Task<T> AsJson<T>(this HttpResponseMessage httpResponseMessage)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}