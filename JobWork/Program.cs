using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobWork
{
    class Program
    {
         static  void Main(string[] args)
        {
           // bool Complate = await MainFunction();

        }

        public async Task<bool> MainFunction()
            {
            List<CourtUser> CourtUsers = new List<CourtUser>();
            using (var context = new DB_A6183C_testbazaEntities())
            {
                CourtUsers = context.CourtUsers.Select(c => c).Where(w => w.ReadyForLogin == true).ToList<CourtUser>();

            }

            foreach (var item in CourtUsers)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://ecd.court.ge");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var formContent = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("UserName", item.UserName),
                         new KeyValuePair<string, string>("Password", item.Password),
                     });
                    //send request
                    HttpResponseMessage responseMessage = await client.PostAsync("/User/Login", formContent);
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);

                }

            }
            return true;

        }

    }
}
