using BLL;
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
            var rslt = await LocationBLL.GetCityByName(search);

            if (rslt != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rslt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 
                    "Could not find the city :" + search);
            }
        }


        [HttpGet]
        public async Task<object> GetLocationFromCoordiantes(double latitude, double longitude)
        {
            var rslt = await LocationBLL.GetNearestCityFromCoordinates(latitude, longitude);

            if (rslt != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rslt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    "Could not find the location");
            }
        }
    }
}
