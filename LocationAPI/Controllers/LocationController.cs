using DAL;
using MDL.Model;
using SAL.Teleport;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocationAPI.Controllers
{

    public enum TypeOfSearch { City, AdminArea, Country }

    public class LocationController : ApiController
    {
        public string Get()
        {
            return "";
        }

        [HttpGet]
        public async Task<object> GetSearchCity(string search)
        {
            var rslt = MainCityDB.FindMainCityByName(search);

            if (rslt != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rslt);
            }
            else
            {
                rslt = await TeleportAPI.Instance.GetCity(search);

                if (rslt != null)
                {
                    rslt.AdminArea = AdminAreaDB.FindAdminAreaByName(rslt.AdminAreaName);
                    rslt.AdminAreaId = rslt.AdminArea.Id;
                    MainCityDB.AddMainCity(rslt);
                    return Request.CreateResponse(HttpStatusCode.OK, rslt);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find the city :" + search);
                }
            }
        }


        [HttpGet]
        public City GetLocationFromCoordiantes(double latitude, double longitude)
        {
            return new City("Post", "Lyon", latitude, longitude, 159874);
        }
    }
}
