
using DL;
namespace MDL.Model
{
    public class City
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public int Population { get; private set; }

        public int AdminAreaId { get; set; }
        public string AdminAreaName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }

        public AdminArea AdminArea { get; set; }
        
        public City(string id, string name, double latitude, double longitude, int population)
        {
            Id = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Population = population;
        }

        public static explicit operator MainCities(City c)
        {
            MainCities mainCities = new MainCities
            {
                Id = c.Id,
                Name = c.Name,
                Population = c.Population,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                AdminArea = c.AdminAreaId,
                AdminAreas =(AdminAreas)c.AdminArea,
            };
            return mainCities;
        }
        public static explicit operator City(MainCities c)
        {
            City city = new City(c.Id, c.Name, c.Latitude ?? 0, c.Longitude ?? 0, c.Population ?? 0)
            {
                AdminAreaId = c.AdminArea,
                AdminArea = (AdminArea)c.AdminAreas,
            };
            return city;
        }

    }
}
