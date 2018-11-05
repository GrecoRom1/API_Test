using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GetTheLocation.ViewModels
{
    public class SearchViewModel
    {
        [Required(ErrorMessage="Please ")]
        public string search { get; set; }
    }

    //public class CoordinatesViewModel
    //{
    //    public double Latitude;
    //    public double Longitude;
    //}
}