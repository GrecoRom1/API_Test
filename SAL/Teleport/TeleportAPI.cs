using MDL.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SAL.Teleport
{
    public class TeleportAPI
    {
        #region Enum
        private enum TypeOfData { Country, City, AdminArea }
        #endregion

        #region Properties
        private static TeleportAPI _instance;
        private static HttpClient client = new HttpClient();
        private const string URL = "https://api.teleport.org/api/";
        #endregion

        #region Constructor
        private TeleportAPI()
        {
            client.BaseAddress = new Uri(URL);
        }

        public static TeleportAPI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeleportAPI();
                }
                return _instance;
            }
        }
        #endregion

        #region Public Methods
       
        /// <summary>
        /// Get all the country from Teleport API
        /// </summary>
        /// <returns>List of MDL.Country</returns>
        public async Task<List<Country>> GetAllCountry()
        {
            List<Country> _list = new List<Country>();
            string urlParameters = "countries";

            // Request API and get response
            var response = GetResultFromTeleportAPI(urlParameters);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body
                var dataObjects = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(dataObjects)["_links"];
                JArray array = (JArray)obj["country:items"];
                
                foreach (var child in array)
                {
                    var href = child["href"].ToString();
                    var name = child["name"].ToString();
                    var id = Parse(TypeOfData.Country, href);
                    _list.Add(new Country(name, id));
                }
                return _list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the AdminArea for a given Country
        /// </summary>
        /// <param name="idCountry"></param>
        /// <returns>List of MDL.AdminArea</returns>
        public async Task<List<AdminArea>> GetAdminAreaFromCountry(string idCountry)
        {
            List<AdminArea> _list = new List<AdminArea>();
            string urlParameters = "countries//" + idCountry + "//admin1_divisions//";                    

            // Request API and get response
            var response = GetResultFromTeleportAPI(urlParameters);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(dataObjects)["_links"];
                JArray array = (JArray)obj["a1:items"];

                foreach (var child in array)
                {
                    var href = child["href"].ToString();
                    var id = Parse(TypeOfData.AdminArea, href);

                    //Exception from the API
                    if (id.Equals("geonames:00"))
                    {
                        return new List<AdminArea>();
                    }

                    var name = child["name"].ToString();

                    _list.Add(new AdminArea(idCountry, id, name));
                }
                return _list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all the name of all Urban Area (= Main city in the world)
        /// </summary>
        /// <returns>List of string</returns>
        public async Task<List<string>> GetAllUrbanArea()
        {
            List<string> _list = new List<string>();
            string urlParameters = "urban_areas";

            var response = GetResultFromTeleportAPI(urlParameters);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(dataObjects)["_links"];
                JArray array = (JArray)obj["ua:item"];

                foreach (var child in array)
                {
                    var name = child["name"].ToString();
                    _list.Add(name);
                }
                return _list;
            }
            else
            {
                return null;
            }
        }
                       
        /// <summary>
        /// Get the information about the city
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>Object MDL.City</returns>
        public async Task<City> GetCity(string cityName)
        {
            
            string id = await GetCityId(cityName);

            if (id == null)
            {
                return null;
            }

            string urlParameters = "cities/" + id;

            var response = GetResultFromTeleportAPI(urlParameters);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(dataObjects)["_links"];
                try
                {
                    string idCountry = Parse(TypeOfData.Country, obj["city:country"]["href"].ToString());                    
                    string nameAdminArea = obj["city:admin1_division"]["name"].ToString();
                    int population = Convert.ToInt32(JObject.Parse(dataObjects)["population"].ToString());

                    var localite = JObject.Parse(dataObjects)["location"];
                    double latitude = Convert.ToDouble(localite["latlon"]["latitude"].ToString());
                    double longitute = Convert.ToDouble(localite["latlon"]["longitude"].ToString());
                   
                    City city = new City(id, cityName, latitude, longitute, population)
                    {
                        CountryId = idCountry,
                        AdminAreaName = nameAdminArea,
                    };
                                       
                    return city;
                }
                catch(Exception e)
                {
                    Debug.WriteLine("TeleportAPI.GetCity() Exception :: " + e.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        //public async Task<Tuple<City, City>> GetLocationFromCoordinates(double latitude, double longitude)
        //{

        //}

        #region Private Methods
        /// <summary>
        /// Request TeleportAPI and get the response
        /// </summary>
        /// <param name="urlParameters"></param>
        /// <returns>HttpResponseMessage of request</returns>
        private HttpResponseMessage GetResultFromTeleportAPI(string urlParameters)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.teleport.v1+json"));
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            return response;
        }

        /// <summary>
        /// Get the City ID from the City name
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>string of the id</returns>
        private async Task<string> GetCityId(string cityName)
        {
            string urlParameters = "cities/?search=" + cityName;

            var response = GetResultFromTeleportAPI(urlParameters);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Parse the response body. Blocking!
                    var dataObjects = await response.Content.ReadAsStringAsync();

                    var obj = JObject.Parse(dataObjects)["_embedded"];
                    JArray array = (JArray)obj["city:search-results"];

                    string rslt = array[0]["_links"]["city:item"]["href"].ToString();

                    return Parse(TypeOfData.City, rslt);
                }
                catch
                {
                    return null;
                }
               
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse a string depending on the type of Data
        /// </summary>
        /// <param name="typeofData"></param>
        /// <param name="href"></param>
        /// <returns>string of the data parsed</returns>
        private string Parse(TypeOfData typeofData, string href)
        {
            string keyword = "";
            switch (typeofData)
            {
                case TypeOfData.Country:
                    keyword = "countries";
                    break;
                case TypeOfData.AdminArea:
                    keyword = "admin1_divisions";
                    break;
                case TypeOfData.City:
                    keyword = "cities";
                    break;
            }
            try
            {
                string[] split = href.Split('/');
                int i = 0;
                while (!split[i].Equals(keyword)) i++;

                return split[i + 1];
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return href;
            }

        }
        #endregion
    }
}

