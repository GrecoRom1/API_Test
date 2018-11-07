using DAL;
using DL;
using SAL;
using System;
using System.Diagnostics;

namespace BLL
{
    public class City
    {
        #region Properties
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public int Population { get; private set; }
               
        public AdminArea AdminArea { get; set; }

        #endregion

        #region Constructor
        public City(string id, string name, double latitude, double longitude, int population)
        {
            Id = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Population = population;
        }
        #endregion

        #region Mapping DB and BLL
        public static explicit operator MainCities(City c)
        {
            MainCities mainCities = new MainCities
            {
                Id = c.Id,
                Name = c.Name,
                Population = c.Population,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                AdminAreas = (AdminAreas)c.AdminArea,
            };
            return mainCities;
        }
        public static explicit operator City(MainCities c)
        {
            try
            {
                City city = new City(c.Id, c.Name, c.Latitude ?? 0, c.Longitude ?? 0, c.Population ?? 0)
                {
                   AdminArea = (AdminArea)AdminAreaDB.GetAdminAreaById(c.AdminArea),
                };
                return city;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
         
        }
        #endregion

        #region Mapping SAL and BLL
        public static explicit operator CitySAL(City c)
        {
            CitySAL mainCities = new CitySAL(c.Id, c.Name, c.Latitude, c.Longitude, c.Population)
            {
                AdminArea = (AdminAreaSAL)c.AdminArea,
            };
            return mainCities;
        }
        public static explicit operator City(CitySAL c)
        {
            City city = new City(c.Id, c.Name, c.Latitude, c.Longitude, c.Population)
            {
                AdminArea = (AdminArea)c.AdminArea,
            };
            return city;
        }
        #endregion

    }
}
