using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using devTest_NikitaOvechenko_client.Helpers;
using devTest_NikitaOvechenko_client.Models;
using Newtonsoft.Json;

namespace devTest_NikitaOvechenko_client
{
    internal class Program
    {
        static string _endpoint;
        static Credentials _credentials;

        const string _defaultUsername = "Supervisor";
        const string _defaultPassword = "Supervisor";

        const string _option0 = "0";
        const string _option1 = "1";
        const string _defaultOption = _option0;
        const string _doOdataRequest = _option0; 

        static StandType _standType;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Русская "А"
            var inputString = "А";
            
            AskStandType();
            AskEndpoint();
            AskCredentials();

            var whatToDo = AskWhatToDo();

            RequestHelper helper = whatToDo == _doOdataRequest
                ? new OdataRequestHelper(_standType, _endpoint, _credentials)
                : new ServiceRequestHelper(_standType, _endpoint, _credentials);

            var result = helper.GetAccountsWithNamePartCount(inputString);

            Console.WriteLine($"Результат: {result}");
        }

        static void AskEndpoint()
        {
            if (_standType == StandType.NetFramework)
            {
                Console.WriteLine("Введи адрес приложения (без /0). Пример: http://localhost:215");
            }
            else
            {
                Console.WriteLine("Введи адрес приложения. Пример: http://localhost:215");
            }

            _endpoint = Console.ReadLine();
        }

        static void AskCredentials()
        {
            Console.WriteLine($"Введи логин (если оставить пустым, будет {_defaultUsername})");
            var username = Console.ReadLine();
            Console.WriteLine($"Введи пароль (если оставить пустым, будет {_defaultPassword})");
            var password = Console.ReadLine();

            username = EnsureFilled(username, _defaultUsername);
            password = EnsureFilled(password, _defaultPassword);

            _credentials = new Credentials() { UserName = username, UserPassword = password };
        }

        static void AskStandType()
        {
            var standType = AskOptional("Введи тип стенда ('{0}' для .Net Framework / '{1}' для .Net Core)");
            _standType = (StandType)int.Parse(standType);
        }

        static string AskWhatToDo()
        {
            return AskOptional("Введи, что хочешь сделать ('{0}' для OData / '{1}' для веб-сервиса)");
        }

        static string AskOptional(string ask)
        {
            var answer = string.Empty;

            do
            {
                var formatedAsk = string.Format(ask, _option0, _option1);
                Console.WriteLine(formatedAsk);
                Console.WriteLine($"(если оставить пустым, будет '{_defaultOption}')");
                answer = Console.ReadLine();
            }
            while (answer != _option0 && answer != _option1 && answer != string.Empty);

            answer = EnsureFilled(answer, _defaultOption);
            return answer;
        }

        static string EnsureFilled(string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value)
                ? defaultValue
                : value;
        }
    }
}
