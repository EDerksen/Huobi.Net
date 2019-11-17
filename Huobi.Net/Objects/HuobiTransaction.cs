using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huobi.Net.Objects
{
    public class HuobiTransaction
    {
        /// <summary>
        /// Transfer id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Define transfer type to search, possible values: [deposit, withdraw]
        /// </summary>
        public HuobiTransactionType Type { get; set; }
        /// <summary>
        /// The crypto currency 
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The on-chain transaction hash
        /// </summary>
        [JsonProperty("tx-hash")]
        public string TxHash { get; set; }
        /// <summary>
        /// The number of crypto asset transfered in its minimum unit
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The deposit or withdraw source address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The user defined address tag
        /// </summary>
        [JsonProperty("address-tag")]
        public string AddressTag { get; set; }
        /// <summary>
        /// The amount of fee taken by Huobi in this crypto's minimum unit
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// The state of this transfer (see below for details)
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// The timestamp in milliseconds for the transfer creation
        /// </summary>
        [JsonProperty("created-at")]
        public long CreatedAt { get; set; }
        /// <summary>
        /// The timestamp in milliseconds for the transfer's latest update
        /// </summary>
        [JsonProperty("updated-at")]
        public long UpdatedAt { get; set; }
    }
}
