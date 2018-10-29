using DL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DAL
{
    public static class CountryDB
    {
        /// <summary>
        /// Get the list of all Country in DB
        /// </summary>
        /// <returns>List of MDL.Country</returns>
        public static List<Countries> GetListCountry()
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.Countries.Select(c => c).ToList();
                    return ctr;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method CountryDB.GetListCountry() Exception :: " 
                        + e.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Add a Country in DB
        /// </summary>
        /// <param name="country"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool AddCountry(Countries country)
        {
            using (var context = new LocationDBEntities())
            {
                var ctr = country;

                try
                {
                    context.Countries.Add(ctr);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method CountryDB.AddCountry() Exception :: " 
                        + e.ToString());
                    return false;
                }
            }
        }

        public static Countries GetCountryFromId(string countryId)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.Countries.Find(countryId);
                    context.Countries.Remove(ctr);
                    return ctr;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method CountryDB.GetCountryFromId() Exception :: "
                        + e.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Delete the specified Country in DB
        /// </summary>
        /// <param name="country"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool DeleteCountry(Countries country)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.Countries.Find(country.GeonameId);
                    context.Countries.Remove(ctr);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method CountryDB.DelteCountry() Exception :: " 
                        + e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Update the info of the specified Country in DB
        /// </summary>
        /// <param name="country"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool UpdateCountry(Countries country)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.Countries.Find(country.GeonameId);
                    ctr.Name = country.Name;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method CountryDB.UpdateCountry() Exception :: " 
                        + e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Drop the Countries table in DB
        /// </summary>
        public static void DeleteCountriesTable()
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM Countries");
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method CountryDB.DeleteCountriesTable() Exception :: " 
                        + e.ToString());
                }
            }
        }
    }
}