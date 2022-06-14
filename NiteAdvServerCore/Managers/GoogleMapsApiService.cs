using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace NiteAdvServerCore.Managers
{
    public static class GoogleMapsApiService 
    {
        static string _googleMapsKey= "AIzaSyD8RpbVlpfwwQq0RXbuc2p4lO-VZtstVyw";

        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private static  HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }


        private async static Task<GooglePlace> GetPlaceDetails(string placeId)
        {
            GooglePlace result = null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/details/json?placeid={Uri.EscapeUriString(placeId)}&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = new GooglePlace(JObject.Parse(json));
                    }
                }
            }

            return result;
        }

        public static async Task<GooglePlace> GetPlaces(string text)
        {

            GooglePlaceAutoCompleteResult placesFound = null;
            GooglePlace result=null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/findplacefromtext/json?input={Uri.EscapeUriString(text)}&inputtype=textquery&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        placesFound = await Task.Run(() =>
                           JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(json)
                        ).ConfigureAwait(false);

                    }
                }
            }
            if (placesFound != null && placesFound.Candidates.Any())
            {
                result = await GetPlaceDetails(placesFound.Candidates[0].PlaceId);

            }
            return result;
        }


    }
}
