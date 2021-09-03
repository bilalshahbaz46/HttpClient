using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                var ticker = "appl";
                var result = await client.GetAsync($"https://datadirect.factset.com/services/DataFetch?report=SEC_REF_DD_LP&id={ticker}&format=xml");


                //loading to XML document
                XmlDocument doc = new XmlDocument();
                doc.Load(await result.Content.ReadAsStreamAsync());
                doc.RemoveChild(doc.FirstChild);


                //Convert to Json String
                var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);


                //Deserializing Json string to C# Object
                var welcom = Test.FromJson(json);


                //Adding The Items into the Dictionary object
                List<Item1> objs = new List<Item1>();

                foreach (DataRow row in welcom.Grid.DataRow)
                {
                    Dictionary<string, string> nimonics = new Dictionary<string, string>();
                    foreach (DataItem item in row.DataItem)
                    {
                        nimonics.Add(item.Name, item.Text);
                    }
                    Item1 obj = new Item1(nimonics, ticker);
                    objs.Add(obj);
                }

                
                Console.WriteLine("Done!");

            }
        }
    }
}
