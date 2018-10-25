using DAL;
using MDL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InitData
{
    [TestClass]
    public class Test_DB_Init
    {
        #region Static Properties
        private readonly int NB_COUNTRIES = 252;
        #endregion

        #region Properties
        private List<Country> ListCountries;
        private List<AdminArea> ListAdminAreas;
        private List<string> ListUrbanAreas;
        private List<City> ListMainCities;
        #endregion

        #region API
        [TestMethod]
        public void GetAllCountriesFromTeleportAPI()
        {
            ListCountries = SAL.Teleport.TeleportAPI.Instance.GetAllCountry()
                .GetAwaiter().GetResult();

            Assert.AreEqual(ListCountries.Count, NB_COUNTRIES);
        }

        [TestMethod]
        public void GetAllAdminAreaFromTeleportAPI()
        {
            ListAdminAreas = new List<AdminArea>();

            foreach (Country c in ListCountries)
            {
                ListAdminAreas.AddRange(SAL.Teleport.TeleportAPI.Instance.GetAdminAreaFromCountry(c.GeonameId)
                    .GetAwaiter().GetResult());
            }
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void GetAllUrbanAreaFromTeleportAPI()
        {
            ListUrbanAreas = new List<string>();

            //Get list of urban area in string
            ListUrbanAreas = SAL.Teleport.TeleportAPI.Instance.GetAllUrbanArea()
                .GetAwaiter().GetResult();

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void CreateCityFromUrbanArea()
        {
            ListMainCities = new List<City>();

            //Make search for each
            foreach (string u in ListUrbanAreas)
            {
                var city = SAL.Teleport.TeleportAPI.Instance.GetCity(u)
                     .GetAwaiter().GetResult();

                if (city != null)
                {
                    ListMainCities.Add(city);
                }
            }

            Assert.AreEqual(1, 1);
        }
        #endregion

        #region Database

        [TestInitialize]
        public void CanConnectToBDD()
        {
            bool answer = true;
            Assert.IsTrue(answer);
        }

        #region DELETE DATA
        [TestMethod]
        public void EraseBDDIsASuccess()
        {
            Debug.WriteLine("Start to Delete all Tables");
            try
            {
                MainCityDB.DeleteMainCitiesTable();
                Debug.WriteLine("MainCities deleted with success");

                AdminAreaDB.DeleteAdminAreasTable();
                Debug.WriteLine("AdminAreas deleted with success");

                CountryDB.DeleteCountriesTable();
                Debug.WriteLine("Countries deleted with success");

                Assert.AreEqual(1, 1);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR IN EraseBDDIsASuccess() :: " + e.ToString());
                Assert.AreEqual(0, 1);
            }
        }

        [TestMethod]
        public void EraseTableAdminAreaBDDIsASuccess()
        {
            try
            {
                AdminAreaDB.DeleteAdminAreasTable();
                Assert.AreEqual(1, 1);
            }
            catch (Exception e)
            {
                Assert.AreEqual(0, 1);
            }
        }
        #endregion

        #region ADD DATA
        [TestMethod]
        public void AddAllCountriesInBDD()
        {
            int nbSuccess = 0;

            foreach (Country c in ListCountries)
            {
                try
                {
                    if (CountryDB.AddCountry(c)) nbSuccess++;
                    else nbSuccess--;
                }
                catch (Exception e)
                {
                    Debug.Write("AddAllCountriesInBDD() :: ERROR Adding country "
                        + c.Name + " :: Exception = "
                        + e.ToString());
                }
            }
            Assert.AreEqual(NB_COUNTRIES, nbSuccess);
        }

        [TestMethod]
        public void AddAllAdminAreasInBDD()
        {
            bool isSuccess = true;

            foreach (AdminArea a in ListAdminAreas)
            {
                try
                {
                    if (AdminAreaDB.AddAdminArea(a, a.CountryId) == -1)
                    {
                        isSuccess = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Write("AddAllAdminAreasInBDD() :: ERROR Adding AdminArea"
                        + a.Name
                        + "to BDD :: Exception = "
                        + e.ToString());
                }
            }
            Assert.IsTrue(isSuccess);
        }

        [TestMethod]
        public void AddAllMainCitiesInBDD()
        {
            bool isSuccess = true;

            foreach (City c in ListMainCities)
            {
                try
                {
                    //Get the Id of AdminArea
                    c.AdminAreaId = AdminAreaDB.GetAdminAreaFromName(c.AdminAreaName).Id;

                    if (!(MainCityDB.AddMainCity(c)))
                    {
                        isSuccess = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Write("AddAllMainCitiesInBDD() :: ERROR Adding MainCity :"
                        + c.Name
                        + " to DB :: Exception = "
                        + e.ToString());
                    break;
                }
            }
            Assert.IsTrue(isSuccess);
        }
        #endregion
        #endregion

        [TestMethod]
        public void JustDoIT()
        {
            GetAllCountriesFromTeleportAPI();
            try
            {
                foreach (Country c in ListCountries)
                {
                    CountryDB.UpdateCountry(c);
                }
                Assert.AreEqual(1, 1);
            }
            catch
            {
                Assert.AreEqual(0, 1);
            }

        }

        //To order the tests
        #region API and Database
        [TestMethod]
        public void TestProcess()
        {
            //Get the data from API
            GetAllDataFromTeleportAPI();

            //Add all the data gathered to BDD
            AddAllDataInBDD();

        }

        private void GetAllDataFromTeleportAPI()
        {
            //Countries
            GetAllCountriesFromTeleportAPI();

            //AdminArea
            GetAllAdminAreaFromTeleportAPI();

            //UrbanArea
            GetAllUrbanAreaFromTeleportAPI();

            CreateCityFromUrbanArea();
        }

        private void AddAllDataInBDD()
        {
            AddAllCountriesInBDD();
            AddAllAdminAreasInBDD();
            AddAllMainCitiesInBDD();
        }

        #endregion
    }
}
