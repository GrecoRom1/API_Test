using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace GetTheLocation.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateSearchEntry(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.MessageErreurSearch = "Please enter a city name";
                return View("Index");
            }
            else if (!(Regex.IsMatch(search, @"^[a-zA-Z_]+$")))
            {
                ViewBag.MessageErreurSearch = "Please enter a correct city name";
                return View("Index");
            }
            else
            {
                return RedirectToAction("Search", "DisplayCity", new { search = search });
            }
        }      
    }
}