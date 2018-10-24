using DL;
using System.Collections.Generic;
namespace MDL.Model
{
    public class AdminArea
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; private set; }
        public string GeonameId { get; private set; }
        public string CountryId { get; private set; }
        public Country Country { get; set; }

        public Dictionary<string, City> ListMainCities { get; set; }
        public Dictionary<string, City> ListSecondaryCities { get; set; }
        #endregion

        #region Constructor
        public AdminArea(string countryId, string geonameId,  string name)
        {
            GeonameId = geonameId;
            CountryId = countryId;
            Name = name;
        }
        #endregion

        #region Mapping DB and MDL
        public static explicit operator AdminAreas(AdminArea a)
        {
            AdminAreas adminAreas = new AdminAreas
            {
                GeonameId = a.GeonameId,
                Name = a.Name,
                Country = a.CountryId,
            };
            return adminAreas;
        }
        public static explicit operator AdminArea(AdminAreas a)
        {
            AdminArea adminArea = new AdminArea(a.Country, a.GeonameId, a.Name)
            {
                Id = a.Id,
            };
            return adminArea;
        }
        #endregion
    }
}
