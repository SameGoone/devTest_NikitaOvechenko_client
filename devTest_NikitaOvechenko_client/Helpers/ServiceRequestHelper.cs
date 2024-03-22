using devTest_NikitaOvechenko_client.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devTest_NikitaOvechenko_client.Helpers
{
    public class ServiceRequestHelper : RequestHelper
    {
        private string ServicePostfixTemplate { get; } = "/rest/{0}/{1}";
        private string ServiceName { get; } = "OvDevTestService";
        private string ServiceMethod { get; } = "GetAccountsWithNamePartCount";

        private string ServiceParameterTemplate { get; } = "?{0}={1}";
        private string ParameterName { get; } = "namePart";

        public ServiceRequestHelper(StandType standType, string endpoint, Credentials credentials) : base(standType, endpoint, credentials) { }

        public override int GetAccountsWithNamePartCount(string namePart)
        {
            var servicePath = StandType == StandType.NetFramework
                ? FrameworkStandPrefix + string.Format(ServicePostfixTemplate, ServiceName, ServiceMethod)
                : string.Format(ServicePostfixTemplate, ServiceName, ServiceMethod);

            var serviceParameter = string.Format(ServiceParameterTemplate, ParameterName, namePart);
            var uri = new Uri(Endpoint + servicePath + serviceParameter);

            var responseContent = SendRequest(uri);

            return int.Parse(responseContent);
        }
    }
}
