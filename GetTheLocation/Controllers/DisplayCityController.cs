using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GetTheLocation.Controllers
{
    public class DisplayCityController : Controller
    {
        // GET: DisplayCity
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<ActionResult> Search(string search)
        {
            var city = await LocationBLL.GetCityByName(search);

            if (city != null)
            {
                return View("DisplayCity", city);
            }
            else
            {
                return View("ErrorCity");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCityFromCoordinates(double latitude, double longitude)
        {
            var city = await LocationBLL.GetNearestCityFromCoordinates(latitude, longitude);

            if (city != null)
            {
                return View("DisplayCity", city);
            }
            else
            {
                return View("ErrorCity");
            }
        }
    }
}