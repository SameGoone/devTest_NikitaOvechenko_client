using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace devTest_NikitaOvechenko_client
{
    internal class Program
    {
        const string _odata4Postfix = "/0/odata";
        const string _authPostfix = "/ServiceModel/AuthService.svc/Login";
        const string _authHeader = "BPMCSRF";
        const string _odataQueryTemplate = "/Account?$count=true&$filter=contains(Name,{0})";

        const string _DoOdataQuery = "odata";
        const string _DoSvcQuery = "svc";

        static HttpClient _client;
        static CookieContainer _cookies;
        static string _endpoint;
        static Credentials _credentials;
        static string _inputString;



        static void Main(string[] args)
        {
            Initialize();

            // Можно расширить до ручного ввода
            _inputString = "A";

            Console.WriteLine("Введи адрес приложения (для .Net Framework без /0). Пример: http://localhost:215");
            _endpoint = Console.ReadLine();

            Console.Write("Введи логин: ");
            var username = Console.ReadLine();
            Console.Write("Введи пароль: ");
            var password = Console.ReadLine();
            _credentials = new Credentials() { UserName = username, UserPassword = password };

            var whatToDo = string.Empty;
            while (whatToDo != "odata" && whatToDo != "svc")
            {
                Console.Write("Введи, что ты хочешь сделать (odata/svc): ");
                whatToDo = Console.ReadLine();
            }

            int result;
            if (whatToDo == "odata")
            {
                result = DoOdataQuery();
            }
            else
            {
                result = DoSvcQuery();
            }
            Console.WriteLine(result);
        }

        static void Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;
            _cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = _cookies;
            _client = new HttpClient(handler);
        }

        private static int DoOdataQuery()
        {
            var query = string.Format(_odataQueryTemplate, _inputString);
            var uri = new Uri(_endpoint + _odata4Postfix + query);

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                uri);

            var authHeaderValue = Authenticate();
            request.Headers.Add(_authHeader, authHeaderValue);

            var response = _client.Send(request);
            response.EnsureSuccessStatusCode();

            Console.WriteLine(authHeaderValue);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static int DoSvcQuery()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private static string Authenticate()
        {
            var a = JsonConvert.SerializeObject(_credentials);
            var content = new StringContent(
                JsonConvert.SerializeObject(_credentials),
                null,
                "application/json");
            var uri = new Uri(_endpoint + _authPostfix);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = content
            };

            var response = _client.Send(request);
            response.EnsureSuccessStatusCode();

            return _cookies
                .GetCookies(uri)
                .First(c => c.Name == _authHeader)
                .Value;
        }
    }
}
