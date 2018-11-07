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
            var cityDAL = MainCityDB.FindMainCityByName(name);

            // City found in Database
            if (cityDAL != null)
            {
                return (City)cityDAL;
            }

            //City is not in database
            else
            {
                //Find city with Teleport API
                var citySAL = await TeleportAPI.Instance.GetCityFromName(name);

                //City found, city is added to DB
                if (citySAL != null)
                {
                    var city = (City)citySAL;
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
            var nearestCity = await TeleportAPI.Instance.GetNearestCityFromCoordinates(latitude, longitude);

            if (nearestCity == null)
            {
                return null;
            }
            else
            {
                var city = (City)nearestCity;
                if ((City)MainCityDB.FindMainCityByName(nearestCity.Name) == null)
                {
                    city.AdminArea = (AdminArea)AdminAreaDB
                            .FindAdminAreaByNameAndCountryId((AdminAreas)city.AdminArea,
                            (Countries)city.AdminArea.Country);

                    MainCityDB.AddMainCity((MainCities)city);
                }

                return city;
            }

        }
    }
}
