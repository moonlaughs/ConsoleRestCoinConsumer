using Newtonsoft.Json;
using RestCoinService.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleRestCoinConsumer
{
    class Program
    {
        static readonly HttpClient Client = new HttpClient();
        static readonly Bid NewBid = new Bid();
        private static string _uri;

        static void Main(string[] args)
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        static async Task RunAsync()
        {
            Console.WriteLine("Write your url here:");
            string url = Console.ReadLine();

            _uri = url;

            Client.BaseAddress = new Uri(url);

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Enter: get/getbyid/stop");

            string httpVerb = Console.ReadLine();
            while (!string.Equals(httpVerb, "stop", StringComparison.Ordinal))
            {
                if (httpVerb != null)
                {
                    IList<Bid> bids;
                    switch (httpVerb.ToUpper())
                    {
                        case "GET":
                            bids = await GetBidAsync();
                            ShowBid(bids);
                            break;
                        case "GETBYID":
                            Console.WriteLine("Write specific id");
                            string id = Console.ReadLine();
                            var itemById = await GetItemByIdAsync(id);
                            ShowBidById(itemById);
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("Enter: get/getbyid/stop");
                httpVerb = Console.ReadLine();
            }
            Console.WriteLine("Program ends! Bye Bye... Press Enter to end the program");
            Console.ReadLine();
        }

        static async Task<IList<Bid>> GetBidAsync()
        {
            string content = await Client.GetStringAsync(_uri);
            IList<Bid> bList = JsonConvert.DeserializeObject<IList<Bid>>(content);
            return bList;
        }

        static async Task<Bid> GetItemByIdAsync(string id)
        {
            string uriId = _uri + "/" + id;
            HttpResponseMessage response = await Client.GetAsync(uriId);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Bid not found. Try another id");
            }
            response.EnsureSuccessStatusCode();
            string str = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<Bid>(str);
            return item;
        }

        static void ShowBid(IList<Bid> bids)
        {
            foreach (var item in bids)
            {
                Console.WriteLine($"ID:{item.Id}, Item: {item.Item}, Price: {item.Price}");
            }
            Console.WriteLine("======================");
        }

        static void ShowBidById(Bid item)
        {
            Console.WriteLine($"ID:{item.Id}, Item: {item.Item}, Price: {item.Price}");
        }
    }
}
