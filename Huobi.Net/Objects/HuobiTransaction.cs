using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huobi.Net.Objects
{
    public class HuobiTransaction
    {
        public int Id;
        public HuobiTransactionType Type;
        public string Currency;
        [JsonProperty("tx-hash")]
        public string TxHash; 
        public decimal Amount; 
        public string Address;
        [JsonProperty("address-tag")]
        public string AddressTag;
        public decimal Fee;
        public string State;
        [JsonProperty("created-at")]
        public long CreatedAt;
        [JsonProperty("updated-at")]
        public long UpdatedAt;
    }
}
