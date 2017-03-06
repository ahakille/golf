using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class AdminModels
    {
        public int id { get; set; }
        [Display(Name = "Golfid")]
        public string GolfID { get; set; }
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; }
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; }
        [Display(Name = "Medlemskategori")]
        public string Membercategory { get; set; }
        [Display(Name = "HCP")]
        public double HCP { get; set; }



    }
}