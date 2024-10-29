using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZhipuApi.Models;
using ZhipuApi.Utils;
using System.Diagnostics;
using ZhipuApi.Models.RequestModels;
using ZhipuApi.Models.ResponseModels.ImageGenerationModels;
using ZhipuApi.Models.ResponseModels.VideoGenerateionModels;

namespace ZhipuApi.Modules
{
    public class Video
    {
        private string _apiKey;
        private static readonly int API_TOKEN_TTL_SECONDS = 60 * 5;
        static readonly HttpClient client = new HttpClient();

        public Video(string apiKey)
        {
            this._apiKey = apiKey;
        }

        private IEnumerable<string> GenerateBase(VideoRequestBase requestBody)
        {
            // Console.WriteLine("----1----");
            var json = JsonSerializer.Serialize(requestBody);
            // Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var api_key = AuthenticationUtils.GenerateToken(this._apiKey, API_TOKEN_TTL_SECONDS);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://open.bigmodel.cn/api/paas/v4/videos/generations"),
                Content = data,
                Headers =
                {
                    { "Authorization", api_key }
                },
                
            };

            var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
            var stream = response.Content.ReadAsStreamAsync().Result;
            byte[] buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                yield return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
        }
        
        public VideoResponseBase Generation(VideoRequestBase requestBody)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in GenerateBase(requestBody))
            {
                sb.Append(str);
            }
            // Console.WriteLine(sb.ToString());

            return VideoResponseBase.FromJson(sb.ToString());
        }


        private IEnumerable<string> GenerateBaseFile(string id)
        {
            // Console.WriteLine("----1----");
            var api_key = AuthenticationUtils.GenerateToken(this._apiKey, API_TOKEN_TTL_SECONDS);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://open.bigmodel.cn/api/paas/v4/async-result/{id}"),
                Headers =
                {
                    { "Authorization", api_key }
                },

            };

            var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
            var stream = response.Content.ReadAsStreamAsync().Result;
            byte[] buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                yield return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
        }

        public VideoResponseData GenerationFile(string id)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in GenerateBaseFile(id))
            {
                sb.Append(str);
            }
            // Console.WriteLine(sb.ToString());

            return VideoResponseData.FromJson(sb.ToString());
        }
    }
}