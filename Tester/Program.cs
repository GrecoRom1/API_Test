using MDL.Model;

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Dictionary<string, Country> listCountry = SAL.Teleport.TeleportAPI.Instance.GetAllCountry().GetAwaiter().GetResult();

            List<string> listCityName = SAL.Teleport.TeleportAPI.Instance.GetAllUrbanArea().GetAwaiter().GetResult();

            foreach(string CityName in listCityName)
            {
               City city = SAL.Teleport.TeleportAPI.Instance.GetCity(CityName).GetAwaiter().GetResult();

                //Country
                //If country entry exists already
                
            }


            Console.ReadLine();
        }

        
    }
}
