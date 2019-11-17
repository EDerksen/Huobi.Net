﻿using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Huobi.Net
{
    internal class HuobiAuthenticationProvider : AuthenticationProvider
    {
        private readonly HMACSHA256 encryptor;
        private readonly object encryptLock = new object();
        private readonly bool signPublicRequests;

        public HuobiAuthenticationProvider(ApiCredentials credentials, bool signPublicRequests) : base(credentials)
        {
            this.signPublicRequests = signPublicRequests;

            if(credentials.Secret == null)
                throw new ArgumentException("ApiKey/secret not provided");

            encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(credentials.Secret.GetString()));
        }

        public override Dictionary<string, object> AddAuthenticationToParameters(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed)
        {
            if (!signed && !signPublicRequests)
                return parameters;

            if (Credentials.Key == null)
                throw new ArgumentException("ApiKey/secret not provided");

            var uriObj = new Uri(uri);
            var signParameters = new Dictionary<string, object>
            {
                { "AccessKeyId", Credentials.Key.GetString() },
                { "SignatureMethod", "HmacSHA256" },
                { "SignatureVersion", 2 }
            };
            
            if(!parameters.ContainsKey("Timestamp") || method != HttpMethod.Get)
                signParameters.Add("Timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));

            if (method == HttpMethod.Get)
            {
                foreach (var kvp in parameters)
                    signParameters.Add(kvp.Key, kvp.Value);
            }

            signParameters = signParameters.OrderBy(kv => Encoding.UTF8.GetBytes(WebUtility.UrlEncode(kv.Key)), new ByteOrderComparer()).ToDictionary(k => k.Key, k => k.Value);
            
            var paramString = signParameters.CreateParamString(true, ArrayParametersSerialization.MultipleValues);
            signParameters = signParameters.OrderBy(kv => kv.Key).ToDictionary(k => k.Key, k => k.Value);

            var absolutePath = uriObj.AbsolutePath;
            if (absolutePath.StartsWith("/api"))
                // Russian api has /api prefix which shouldn't be part of the signature
                absolutePath = absolutePath.Substring(4);

            var signData = method + "\n";
            signData += uriObj.Host + "\n";
            signData += absolutePath + "\n";
            signData += paramString;
            byte[] signBytes;
            lock(encryptLock)
                signBytes = encryptor.ComputeHash(Encoding.UTF8.GetBytes(signData));
            signParameters.Add("Signature", Convert.ToBase64String(signBytes));

            if (method != HttpMethod.Get)
            {
                foreach (var kvp in parameters)
                    signParameters.Add(kvp.Key, kvp.Value);
            }

            return signParameters;
        }
    }
}
