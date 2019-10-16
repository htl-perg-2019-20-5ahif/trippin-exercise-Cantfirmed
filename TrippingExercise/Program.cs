using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrippingExercise {
    public static partial class Program {
        
        private static HttpClient Client
            = new HttpClient() { BaseAddress = new Uri("https://www.odata.org/odata-services/TripPinRESTierService/(S(vefsn0uv1rynvd2bh2wjv3xl))") };

        public static async Task UserExists(User user) {
            if (!(await Client.GetAsync($"People('{user.UserName}')")).IsSuccessStatusCode) {
                await Client.PostAsync("People", new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(new { user.UserName, user.FirstName, user.LastName, Emails = new[] { user.Email }, AddressInfo = new[] { new { user.Address, City = new { Name = user.CityName, CountryRegion = user.Country, Region = "unknown" } } } }), System.Text.Encoding.UTF8));
                Console.WriteLine("Added user: " + user.UserName);
            }
        }

        public static async Task Main(string[] args) {
            User[] users = JsonSerializer.Deserialize<User[]>(File.ReadAllText("users.json"));
            Console.WriteLine("Starting to add unknown users");
            foreach(User user in users) {
                await UserExists(user);
            }
            Console.WriteLine("Adding unknown users finished");
        }
    }
}
