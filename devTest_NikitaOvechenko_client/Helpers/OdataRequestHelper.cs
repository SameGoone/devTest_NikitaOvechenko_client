using devTest_NikitaOvechenko_client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devTest_NikitaOvechenko_client.Helpers
{
    public class OdataRequestHelper : RequestHelper
    {
        private string Odata4Postfix { get; } = "/odata";
        private string OdataQueryTemplate { get; } = "/Account?$count=true&$filter=contains(Name,'{0}')";

        public OdataRequestHelper(StandType standType, string endpoint, Credentials credentials) : base(standType, endpoint, credentials) { }

        public override int GetAccountsWithNamePartCount(string namePart)
        {
            var query = string.Format(OdataQueryTemplate, namePart);

            var uri = StandType == StandType.NetFramework
                ? new Uri(Endpoint + FrameworkStandPrefix + Odata4Postfix + query)
                : new Uri(Endpoint + Odata4Postfix + query);

            var responseContent = SendRequest(uri);

            var odataResponse = JsonConvert.DeserializeObject<OdataResponse>(responseContent);
            return odataResponse.OdataCount;
        }
    }
}
