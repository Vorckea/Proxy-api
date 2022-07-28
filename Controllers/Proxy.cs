using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using mTerms_Proxy_api;
using Newtonsoft.Json;

namespace mTerms_Proxy_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Proxy : ControllerBase
    {
        private static readonly HttpClient _httpClient;

        private static string Link { get; set; }
        private static string Username { get; set; }
        private static string Password { get; set; }

        static Proxy()
        {
            var socketHandler = new SocketsHttpHandler
            {  
            };
            _httpClient = new HttpClient(socketHandler);


            Task.Run(async () =>
            {
                var json = string.Empty;
                using (var fs = System.IO.File.OpenRead("Config.json"))
                using (var sr = new StreamReader(fs, false))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);

                var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
                Link = configJson.Link;
                Username = configJson.Username;
                Password = configJson.Password;
            });


        }

        //For more parameters add them in GetStatement(string query, <Type> <Parameter>)
        //And in _httpClient.GetAsync($"{Link}{query}{Parameter}")
        [HttpGet("details")]
        public async Task<string> GetStatement(string query)
        {
            //Authorization
            var byteArray = Encoding.ASCII.GetBytes($"{Username}:{Password}");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            
            //Response
            HttpResponseMessage response = await _httpClient.GetAsync($"{Link}/{query}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        [HttpPost("details")]
        public async Task<string> PostStatement(string query, HttpContent content)
        {
            //Authorization
            var byteArray = Encoding.ASCII.GetBytes($"{Username}:{Password}");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            //Response
            HttpResponseMessage response = await _httpClient.PostAsync($"{Link}{query}", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
