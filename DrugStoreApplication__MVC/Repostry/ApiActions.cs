using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Repostry
{
    public class ApiActions : IApiActions
    {

        public async Task<string> PostToAPi(string path , object obj)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PostAsJsonAsync(path, obj);
            if (response.IsSuccessStatusCode == true)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        public async Task<string> GetFromApi(string path)
        {
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization =
            //              new AuthenticationHeaderValue("Bearer", token );
            HttpResponseMessage response = await client.GetAsync(path );
            if (response.IsSuccessStatusCode == true)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        public async Task<string>DeleteFromApi(string path)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44321/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.DeleteAsync(path);
            if (response.IsSuccessStatusCode == true)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
