﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace beam_client_csharp
{
    public class BeamWeb
    {
        private CookieContainer _cookieContainer;

        public async Task<BeamUser.BeamUser> Authenticate(string username, string password)
        {
            _cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler {CookieContainer = _cookieContainer})
            using (var client = new HttpClient(handler))
            {
                var values = new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password}
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://beam.pro/api/v1/users/login", content);
                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseString);
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
                if (result.ContainsKey("statusCode"))
                    return null;

                return JsonConvert.DeserializeObject<BeamUser.BeamUser>(responseString);
            }
        }

        public async Task<BeamChatInfo> ChatInfo(int channelId)
        {
            if (_cookieContainer == null) _cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler {CookieContainer = _cookieContainer})
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync("https://beam.pro/api/v1/chats/" + channelId);
                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseString);

                return JsonConvert.DeserializeObject<BeamChatInfo>(responseString);
            }
        }
    }
}