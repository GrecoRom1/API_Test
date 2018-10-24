using DL;
using MDL.Model;
using System;
using System.Diagnostics;

namespace DAL
{
    public static class MainCityDB
    {
        /// <summary>
        /// Add a MainCity in DB
        /// </summary>
        /// <param name="city"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool AddMainCity(City city)
        {
            using (var context = new LocationDBEntities())
            {
                var ctr = (MainCities)city;

                try
                {
                    context.MainCities.Add(ctr);
                    context.SaveChanges();
                    return true;
                }

                catch (Exception e)
                {
                    Debug.WriteLine("Method MainCityDB.AddMainCity() Exception :: " 
                        + e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete the specified MainCity in DB
        /// </summary>
        /// <param name="city"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool DeleteMainCity(City city)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.MainCities.Find(city.Id);
                    context.MainCities.Remove(ctr);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method MainCityDB.DeleteMainCity() Exception :: " +
                        e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Update the MainCity in DB
        /// </summary>
        /// <param name="city"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool UpdateMainCity(City city)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.MainCities.Find(city.Id);
                    ctr = (MainCities)city;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method MainCityDB.UpdateMainCity() Exception :: " 
                        + e.ToString());
                    return false;
                }

            }
        }

        /// <summary>
        /// Drop the MainCities table in DB
        /// </summary>
        public static void DeleteMainCitiesTable()
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM MainCities");
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method MainCityDB.DeleteMainCitiesTable() Exception :: " 
                        + e.ToString());
                }
            }
        }
    }
}