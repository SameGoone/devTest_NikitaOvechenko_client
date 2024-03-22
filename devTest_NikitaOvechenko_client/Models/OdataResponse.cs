using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devTest_NikitaOvechenko_client.Models
{
    internal class OdataResponse
    {
        [JsonProperty("@odata.count")]
        public int OdataCount { get; set; }
    }
}
