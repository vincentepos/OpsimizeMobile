using Newtonsoft.Json;
using OM.Models;
using OM.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OM.Data
{
    public class RestGetToken
    {
        HttpClient client;

        public RestGetToken()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<Tokens> Login(User user)
        {
            object userInfos = new {user.Username, user.Password };
            var jsonObj = JsonConvert.SerializeObject(userInfos);
            using (client)
            {
                string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username + ":" + user.Password));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
                try
                {
                    var response = await client.GetStringAsync(Constants.LoginUrl);
                    Console.WriteLine(response);
                    Tokens result = JsonConvert.DeserializeObject<Tokens>(response);
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
                
            }
        }
    }
}
