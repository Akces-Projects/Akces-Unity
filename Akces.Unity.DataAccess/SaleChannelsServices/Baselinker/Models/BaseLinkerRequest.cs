using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace Akces.Unity.DataAccess.Services.Baselinker.Models
{
    public class BaseLinkerRequest : HttpRequestMessage
    {
        public string BaselinkerMethod { get; }
        public Dictionary<string, object> Parameters { get; }

        public BaseLinkerRequest(HttpMethod httpMethod, Uri baseAddress, string method, string token) : base(httpMethod, baseAddress)
        {
            BaselinkerMethod = method;
            Parameters = new Dictionary<string, object>();
            Headers.Add("Accept", "application/json");
            Headers.Add("X-BLToken", token);
            BuildContent();
        }

        public void SetParameter(string key, object value)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = value;
            }
            else
            {
                Parameters.Add(key, value);
            }

            BuildContent();
        }
        private void BuildContent()
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["method"] = BaselinkerMethod,
                ["parameters"] = JsonSerializer.Serialize(Parameters)
            });
        }
    }
}
