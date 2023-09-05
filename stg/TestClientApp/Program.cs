using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            var response = client.PostAsync("http://localhost:65489/api/Auth/login",
                new StringContent(JsonConvert.SerializeObject(new System.Collections.Generic.Dictionary<string, string>() { { "Username", "admin" }, { "Password", "admin" } }),
                Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;

            dynamic objectresult = JsonConvert.DeserializeObject(response);
            string token = objectresult["token"];

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response2 = client.GetStringAsync("http://localhost:49232/api/values").Result;


            Console.ReadKey();
        }
    }
}
