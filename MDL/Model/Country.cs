using System.Collections.Generic;
using DL;

namespace MDL.Model
{

    public class Country
    {
        public string Name { get; set; }
        public string GeonameId { get; set; }

        public Dictionary<string, AdminArea> ListAdminAreas;

        public Country(string name, string geonameId)
        {
            Name = name;
            GeonameId = geonameId;
        }

        public static explicit operator Countries(Country c)
        {
            Countries countries = new Countries
            {
                GeonameId = c.GeonameId,
                Name = c.GeonameId
            };
            return countries;
        }
        public static explicit operator Country(Countries c)
        {
            Country country = new Country(c.GeonameId, c.Name);
            return country;
        }


    }
}
