using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace WesleyUntity
{
    /// <summary>
    /// OCR认证
    /// </summary>
    public static class BaiduOCRHelper
    {
        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="clientId">百度AK</param>
        /// <param name="clientSecret">百度SK</param>
        /// <param name="file">base64</param>
        /// <returns>身份证号码</returns>
        public static string GetIdCard(string clientId, string clientSecret, string file)
        {
            var token = GetAccessToken(clientId, clientSecret);
            var host = "https://aip.baidubce.com/rest/2.0/ocr/v1/idcard?access_token=" + token;
            var request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            var str = "id_card_side=" + "front" + "&image=" + HttpUtility.UrlEncode(file);
            var buffer = Encoding.UTF8.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var result = reader.ReadToEnd();
            dynamic m = JsonConvert.DeserializeObject<dynamic>(result);
            return m.words_result.公民身份号码.words;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        private static string GetAccessToken(string clientId, string clientSecret)
        {
            string authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            };

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic m = JsonConvert.DeserializeObject<dynamic>(result);
            return m.access_token;
        }

        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object GetMessageByOCR(string clientId, string clientSecret, string file)
        {
            var token = GetAccessToken(clientId, clientSecret);
            var host = "https://aip.baidubce.com/rest/2.0/ocr/v1/idcard?access_token=" + token;
            var request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            var str = "id_card_side=" + "front" + "&image=" + HttpUtility.UrlEncode(file);
            var buffer = Encoding.UTF8.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var result = reader.ReadToEnd();
            dynamic m = JsonConvert.DeserializeObject<dynamic>(result);
            return m;
        }
    }
}