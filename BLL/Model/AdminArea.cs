using DAL;
using DL;
using SAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BLL
{
    public class AdminArea
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; private set; }
        public string GeonameId { get; private set; }
        //public string CountryId { get; private set; }
        public Country Country { get; set; }

        public Dictionary<string, City> ListMainCities { get; set; }
        public Dictionary<string, City> ListSecondaryCities { get; set; }
        #endregion

        #region Constructor
        public AdminArea(string name, string geonameId, Country country)
        {
            GeonameId = geonameId;
            Name = name;
            Country = country;
        }
        #endregion

        #region Mapping DB and BLL

        //BLL to DB
        public static explicit operator AdminAreas(AdminArea a)
        {
            AdminAreas adminAreas = new AdminAreas
            {
                GeonameId = a.GeonameId,
                Name = a.Name,
                //Country = a.CountryId,
                Countries = (Countries)a.Country,
            };
            return adminAreas;
        }

        //DB to BLL
        public static explicit operator AdminArea(AdminAreas a)
        {
            try
            {
                AdminArea adminArea = new AdminArea(a.Name, a.GeonameId, (Country)CountryDB.GetCountryFromId(a.Country));                
                return adminArea;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
           
        }
        #endregion

        #region Mapping SAL and BLL
        public static explicit operator AdminAreaSAL(AdminArea a)
        {
            AdminAreaSAL adminAreas = new AdminAreaSAL()
            {
                Id = a.Id,
                Name = a.Name,
                GeonameId = a.GeonameId,
                Country = (CountrySAL)a.Country,
            };
            return adminAreas;
        }

        public static explicit operator AdminArea(AdminAreaSAL a)
        {
            AdminArea adminArea = new AdminArea(a.Name, a.GeonameId, (Country)a.Country)
            {
                Id = a.Id,
            };
            return adminArea;
        }
        #endregion
    }
}
