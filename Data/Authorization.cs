using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using SpotifyTypes.MetaData;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Runtime.Caching;
using Newtonsoft.Json;

namespace Data
{
    public class Authorization : IAuthorization
    {
        private ILogger _logger;
        private HttpClient _authClient;

        private static string _userClientID = Environment.GetEnvironmentVariable("SpotifyClientID");
        private static string _userClientSecret = Environment.GetEnvironmentVariable("SpotifyClientSecret");
        private static string _refreshToken = Environment.GetEnvironmentVariable("SpotifyRefreshToken");

        public Authorization(ILogger log)
        {
            _logger = log;
            _authClient = new HttpClient();
        }

        public Token GetToken()
        {
            Token returnToken;

            TokenLog("Start");
            TokenLog("Checking MemoryCache");

            if (MemoryCache.Default.Get("MemToken") == null)
            {
                TokenLog("Not in Cache");

                string bothClientInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(_userClientID + ":" + _userClientSecret));

                _authClient.DefaultRequestHeaders.Add("Authorization", $"Basic {bothClientInfo}");

                Dictionary<string, string> bodyArgs = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token"},
                    { "refresh_token", _refreshToken}
                };

                FormUrlEncodedContent tokenRequestBody = new FormUrlEncodedContent(bodyArgs);

                TokenLog("Send");

                HttpResponseMessage refreshResponse = _authClient.PostAsync("https://accounts.spotify.com/api/token", tokenRequestBody).GetAwaiter().GetResult();

                if (!refreshResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Error getting Spotify Bearer Token: {refreshResponse.ReasonPhrase}");
                }

                string returnMessage = refreshResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                TokenLog("Recieved");

                returnToken = JsonConvert.DeserializeObject<Token>(returnMessage);
                                
                MemoryCache.Default.Set("MemToken", returnToken, DateTimeOffset.Now.AddMinutes(59));

                TokenLog("Added Token to Memory");
            }
            else
            {
                TokenLog("Retrieve from Cache");
                returnToken = (Token)MemoryCache.Default.Get("MemToken");
            }

            TokenLog("Return");
            return returnToken;
        }

        private void TokenLog(string logString)
        {
            _logger.LogInformation($"GetToken - {logString} - {DateTime.Now}");
        }
    }
}
