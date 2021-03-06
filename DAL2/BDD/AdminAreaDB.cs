﻿using DL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DAL
{
    public static class AdminAreaDB
    {
        /// <summary>
        /// Get the list of all AdminAreas in the world
        /// </summary>
        /// <returns>List of MDL.AdminArea</returns>
        public static List<AdminAreas> GetListAdminAreas()
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var admin = context.AdminAreas.Select(a => a).ToList();
                    return admin;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.GetListAdminAreas() Exception :: " + e.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Add a new AdminArea in DB
        /// </summary>
        /// <param name="adminArea"></param>
        /// <param name="countryId"></param>
        /// <returns>The Database ID generated by the DB</returns>
        public static int AddAdminArea(AdminAreas adminArea, string countryId)
        {
            using (var context = new LocationDBEntities())
            {
                var admin = adminArea;
                admin.Country = countryId;
                try
                {
                    context.AdminAreas.Add(admin);
                    context.SaveChanges();
                    return admin.Id;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.AddAdminArea() Exception :: " + e.ToString());
                    return -1;
                }
            }
        }

        public static AdminAreas GetAdminAreaById(int id)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var admin = context.AdminAreas.Find(id);
                    return admin;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.GetAdminAreaById() Exception :: " + e.ToString());
                    return null;
                }
            }
        }

        public static AdminAreas FindAdminAreaByNameAndCountryId(AdminAreas adminArea, Countries country)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var ada = context.AdminAreas
                        .Where(a => ((a.Name == adminArea.Name) && 
                        (a.Countries.GeonameId == country.GeonameId)))
                        .FirstOrDefault();
                    return ada;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.FindAdminAreaByName() Exception :: " + e.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Delete the specified AdminArea
        /// </summary>
        /// <param name="adminArea"></param>
        /// <returns>true if OK or false if error</returns>
        public static bool DeleteAdminArea(AdminAreas adminArea)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var admin = context.AdminAreas.Find(adminArea.Id);
                    context.AdminAreas.Remove(admin);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.DeleteAdminArea() Exception :: " + e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Update the AdminArea in DB
        /// </summary>
        /// <param name="adminArea"></param>
        /// <returns>true if OK or false if error</returns>
        public static bool UpdateAdminArea(AdminAreas adminArea)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var admin = context.AdminAreas.Find(adminArea.Id);
                    admin = adminArea;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.UpdateAdminArea() Exception :: " + e.ToString());
                    return false;
                }

            }
        }

        /// <summary>
        /// Drop the AdminAreas table in DB
        /// </summary>
        public static void DeleteAdminAreasTable()
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM AdminAreas");
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.DeleteAdminAreasTable() Exception :: " 
                        + e.ToString());
                }
            }
        }

        /// <summary>
        /// Return the first AdminArea which match with the adminAreaName passed in parameter
        /// </summary>
        /// <param name="adminAreaName"></param>
        /// <returns>The first MDL.AdminArea that matches</returns>
        public static AdminAreas GetAdminAreaFromName(string adminAreaName)
        {
            using (var context = new LocationDBEntities())
            {
                try
                {
                    var admin = context.AdminAreas.
                        Where(a => a.Name == adminAreaName)
                        .FirstOrDefault<AdminAreas>();
                    return admin;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Method AdminAreaDB.GetAdminAreaFromName() Exception :: " 
                        + e.ToString());
                    return null;
                }
            }
        }
    }
}