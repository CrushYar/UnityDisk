using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityDisk.Accounts;
using UnityDisk.FileStorages.OneDrive.Deserialized;
using Microsoft.Identity.Client;

namespace UnityDisk.FileStorages.OneDrive
{
    [Serializable]
    public sealed class Account : IFileStorageAccount
    {
        public string Login { get; set; }
        public string Id { get; set; }
        public SpaceSize Size { get; set; }
        public string ServerName => "OneDrive";
        public string Token { get; set; }
        public ConnectionStatusEnum Status { get; set; }

        public async Task SignIn(string key)
        {
            var authenticationResult = await App.PublicClientApp.AcquireTokenSilentAsync(App.Scopes,
                App.PublicClientApp.Users.FirstOrDefault(user => user.DisplayableId== key));

            Token = authenticationResult.AccessToken;
            using (var stream = await GetDataStream("https://graph.microsoft.com/v1.0/me"))
            {
                DeserializedAccount deserializedAccount = new DeserializedAccount();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedAccount.GetType());
                deserializedAccount = ser.ReadObject(stream) as DeserializedAccount;
                if (deserializedAccount == null) throw new NullReferenceException("Couldn't deserialized the data");
                Login = deserializedAccount.userPrincipalName;
                Id = deserializedAccount.id;
            }
            using (var stream = await GetDataStream("https://graph.microsoft.com/v1.0/me/drive"))
            {
                DeserializedAccount deserializedAccount = new DeserializedAccount();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedAccount.GetType());
                deserializedAccount = ser.ReadObject(stream) as DeserializedAccount;
                if (deserializedAccount == null) throw new NullReferenceException("Couldn't deserialized the data");
                Size=new SpaceSize();
                Size.TotalSize = deserializedAccount.quota.total;
                Size.UsedSize = deserializedAccount.quota.used;
                Size.FreelSize = deserializedAccount.quota.remaining;
            }

            Status = ConnectionStatusEnum.Connected;
        }
        public Task SignOut()
        {
            throw new NotImplementedException();
        }
        public async Task Update()
        {
            using (var stream = await GetDataStream("https://graph.microsoft.com/v1.0/me/drive"))
            {
                DeserializedAccount deserializedAccount = new DeserializedAccount();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedAccount.GetType());
                deserializedAccount = ser.ReadObject(stream) as DeserializedAccount;
                if (deserializedAccount == null) throw new NullReferenceException("Couldn't deserialized the data");
                Size = new SpaceSize();
                Size.TotalSize = deserializedAccount.quota.total;
                Size.UsedSize = deserializedAccount.quota.used;
                Size.FreelSize = deserializedAccount.quota.remaining;
            }
            Status = ConnectionStatusEnum.Connected;
        }
        public IFileStorageAccount Clone()
        {
            return new Account(){Login = Login,Id = Id, Size = new SpaceSize(Size), Status = Status, Token = Token};
        }
        private async Task<System.IO.Stream> GetDataStream(string url)
        {
            var httpClient = new System.Net.Http.HttpClient();
            
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
            string test= await response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
