using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestingSomething
{
    class Program
    {
        static void Main(string[] args)
        {
            CallAPI().Wait();
        }

        static async Task CallAPI()
        {
            var userName = "LP_OMS_1232158_SERVICES";
            var password = "UgZ6tUdu6Y6zy8Tw";
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{password}");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                //Get Request
                var result = await client.GetAsync($"https://datadirect.factset.com/services/DataFetch?report=SEC_REF_DD_LP&id=appl&format=xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(await result.Content.ReadAsStreamAsync());

                var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);

                var Items = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

                Console.WriteLine(Items);
            }
        }
    }
}
