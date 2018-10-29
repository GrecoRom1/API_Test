using DL;
using System;
using System.Diagnostics;
using System.Linq;

namespace DAL
{
    public static class MainCityDB
    {
        /// <summary>
        /// Add a MainCity in DB
        /// </summary>
        /// <param name="city"></param>
        /// <returns>true if OK or false if ERROR</returns>
        public static bool AddMainCity(MainCities city)
        {
            using (var context = new LocationDBEntities())
            {
                var ctr = city;

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
        public static bool DeleteMainCity(MainCities city)
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
        public static bool UpdateMainCity(MainCities city)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.MainCities.Find(city.Id);
                    ctr = city;
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

        public static MainCities FindMainCityByName(string name)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ctr = context.MainCities.Where(c=>c.Name == name).FirstOrDefault();                  
                    return ctr;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method MainCityDB.FindMainCityByName() Exception :: "
                        + e.ToString());
                    return null;
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