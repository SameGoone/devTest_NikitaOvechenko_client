using devTest_NikitaOvechenko_client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace devTest_NikitaOvechenko_client.Helpers
{
    public abstract class RequestHelper
    {
        protected string AuthPostfix { get; } = "/ServiceModel/AuthService.svc/Login";
        protected string AuthHeader { get; } = "BPMCSRF";
        protected string FrameworkStandPrefix { get; } = "/0";

        protected HttpClient Client { get; set; }
        protected CookieContainer Cookies { get; set; }
        protected Credentials Credentials { get; set; }
        protected StandType StandType { get; set; }
        protected string Endpoint { get; set; }

        public RequestHelper(StandType standType, string endpoint, Credentials credentials)
        {
            StandType = standType;
            Endpoint = endpoint;
            Credentials = credentials;
            Initialize();
        }

        public abstract int GetAccountsWithNamePartCount(string namePart);

        protected virtual void Initialize()
        {
            Cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = Cookies;
            Client = new HttpClient(handler);
        }

        protected virtual string SendRequest(Uri uri)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                uri);

            var authHeaderValue = Authenticate();
            request.Headers.Add(AuthHeader, authHeaderValue);

            var response = Client.Send(request);
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        protected virtual string Authenticate()
        {
            var a = JsonConvert.SerializeObject(Credentials);
            var content = new StringContent(
                JsonConvert.SerializeObject(Credentials),
                null,
                "application/json");
            var uri = new Uri(Endpoint + AuthPostfix);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = content
            };

            var response = Client.Send(request);
            response.EnsureSuccessStatusCode();

            return Cookies
                .GetCookies(uri)
                .First(c => c.Name == AuthHeader)
                .Value;
        }
    }
}
