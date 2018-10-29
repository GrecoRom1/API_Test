using System;
using System.Collections.Generic;
using System.Diagnostics;
using DL;
using SAL;

namespace BLL
{
    public class Country
    {
        #region Properties
        public string Name { get; set; }
        public string GeonameId { get; set; }
        public Dictionary<string, AdminArea> ListAdminAreas;
        #endregion

        #region Constructor
        public Country(string name, string geonameId)
        {
            Name = name;
            GeonameId = geonameId;
        }
        #endregion

        #region Mapping DB and BLL
        public static explicit operator Countries(Country c)
        {
            Countries countries = new Countries
            {
                GeonameId = c.GeonameId,
                Name = c.Name
            };
            return countries;
        }
        public static explicit operator Country(Countries c)
        {
            try
            {
                Country country = new Country(c.Name, c.GeonameId);
                return country;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
           
        }
        #endregion

        #region Mapping SAL and BLL
        public static explicit operator CountrySAL(Country c)
        {
            CountrySAL countries = new CountrySAL(c.Name, c.GeonameId);           
            return countries;
        }
        public static explicit operator Country(CountrySAL c)
        {
            Country country = new Country(c.Name, c.GeonameId);
            return country;
        }
        #endregion

    }
}
