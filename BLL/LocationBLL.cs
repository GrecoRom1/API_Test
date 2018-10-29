using DAL;
using DL;
using SAL.Teleport;
using System.Threading.Tasks;

namespace BLL
{
    public static class LocationBLL
    {
        public async static Task<City> GetCityByName(string name)
        {
            var city = (City)MainCityDB.FindMainCityByName(name);

            // City found in Database
            if (city != null)
            {
                return city;
            }

            //City is not in database
            else
            {
                //Find city with Teleport API
                city = (City)await TeleportAPI.Instance.GetCityFromName(name);

                //City found, city is added to DB
                if (city != null)
                {
                    city.AdminArea = (AdminArea)AdminAreaDB
                        .FindAdminAreaByNameAndCountryId((AdminAreas)city.AdminArea,
                        (Countries)city.AdminArea.Country);

                    MainCityDB.AddMainCity((MainCities)city);
                    return city;
                }

                //No city found
                else
                {
                    return null;
                }
            }
        }

        public async static Task<City> GetNearestCityFromCoordinates(double latitude, double longitude)
        {
            City nearestCity = (City) await TeleportAPI.Instance.GetNearestCityFromCoordinates(latitude, longitude);

            if ((City)MainCityDB.FindMainCityByName(nearestCity.Name) == null)
            {
                nearestCity.AdminArea = (AdminArea)AdminAreaDB
                        .FindAdminAreaByNameAndCountryId((AdminAreas)nearestCity.AdminArea,
                        (Countries)nearestCity.AdminArea.Country);

                MainCityDB.AddMainCity((MainCities)nearestCity);
            }

            return nearestCity;

        }
    }
}
