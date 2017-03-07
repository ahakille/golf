using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class AdminModels
    {
        public class Adminviewmodel
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
            [Display(Name = "Kön")]
            public string Gender { get; set; }

        }
        public int ID { get; set; }
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; }
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; }
        [Display(Name = "Adress")]
        public string Adress { get; set; }
        [Display(Name = "Postkod")]
        public string Postalcode { get; set; }
        [Display(Name = "Ort")]
        public string City { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Kön")]
        public List<GenderModels> Gender { get; set; }
        [Display(Name = "HCP")]
        public double HCP { get; set; }
        [Display(Name = "Golfid")]
        public string GolfID { get; set; }
        [Display(Name = "Medlemskategori")]
        public List<MembercategoryModels> Membercategory { get; set; }
        [Display(Name = "Telefonnummer")]
        public string Telephone { get; set; }
        [Display(Name = "Betalat medlemsavgift")]
        public bool Payment { get; set; } = false;
    }

    
}