using Newtonsoft.Json;
using OM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OM.Data
{
    public class RestGetPO
    {
        HttpClient client;

        public RestGetPO()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<PO> GetPO(User user, PO po)
        {
            object userInfos = new { user.Username, user.Password };
            var jsonObj = JsonConvert.SerializeObject(userInfos);
            using (client)
            {
                string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username + ":" + user.Password));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
                var response = await client.GetStringAsync(Constants.LoginUrl + "?PORef=" + "1YWDJA");
                Console.WriteLine(response);
                PO result = JsonConvert.DeserializeObject<PO>(response);
                return result;
            }
        }
    }
}
