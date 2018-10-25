using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        // GET api/location
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/location/search
        [HttpGet("{search:regex(^search=[[A-z]]{{1,20}}$)}")]
        public string Get(string search)
        {
            var match = Regex.Match(search, @"^search=(?<search>[A-z]{1,20})$");
            return match.Groups["search"].Value;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
