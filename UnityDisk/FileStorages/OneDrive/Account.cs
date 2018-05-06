using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityDisk.Accounts;

namespace UnityDisk.FileStorages.OneDrive
{
    [Serializable]
    [DataContract]
    public sealed class Account : IFileStorageAccount
    {
        [JsonProperty(PropertyName = "userPrincipalName")]
        public string Login { get; set; }
        public SpaceSize Size { get; set; }
        public string ServerName { get; set; }
        public string Token { get; set; }
        public ConnectionStatusEnum Status { get; set; }
        public async Task SignIn(string key)
        {
            using (var stream = await GetDataStream(key))
            {
                Account deserializedAccount = new Account();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedAccount.GetType());
                deserializedAccount = ser.ReadObject(stream) as Account;
                if(deserializedAccount==null) throw new NullReferenceException("Couldn't deserialized the data");
                Login = deserializedAccount.Login;
                Login = deserializedAccount.Login;
            }
        }

        public Task SignOut()
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }
        public IFileStorageAccount Clone()
        {
            throw new NotImplementedException();
        }

        private async Task<System.IO.Stream> GetDataStream(string url)
        {
            var httpClient = new System.Net.Http.HttpClient();
            
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
           return await response.Content.ReadAsStreamAsync();
        }
    }
}
