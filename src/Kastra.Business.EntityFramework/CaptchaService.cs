using Kastra.Business.DTO;
using Kastra.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kastra.Business
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CaptchaService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> Verify(string secret, string token, string remoteIp)
        {
            HttpClient client = _clientFactory.CreateClient("hCaptcha");

            // Create post data
            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("secret", secret),
                new KeyValuePair<string, string>("response", token),
                new KeyValuePair<string, string>("remoteip", remoteIp)
            };

            // Request api
            HttpResponseMessage response = await client.PostAsync("/siteverify", new FormUrlEncodedContent(postData));

            response.EnsureSuccessStatusCode();

            Stream contentStream = await response.Content.ReadAsStreamAsync();

            try
            {
                CaptchaResult result = await JsonSerializer.DeserializeAsync<CaptchaResult>(contentStream, new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });

                return result.Success;
            }
            catch (JsonException)
            {
                Console.WriteLine("Invalid JSON.");
            }

            return false;
        }
    }
}
