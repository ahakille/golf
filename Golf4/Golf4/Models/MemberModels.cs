using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class MemberModels
    {
        public class MembersViewModel
        {
            public int ID { get; set; }
            [Display(Name = "Förnamn")]
            public string Firstname { get; set; } = "";
            [Display(Name = "Efternamn")]
            public string Lastname { get; set; } = "";
            [Display(Name = "Adress")]
            public string Adress { get; set; } = "";
            [Display(Name = "Postkod")]
            public string Postalcode { get; set; } = "";
            [Display(Name = "Ort")]
            public string City { get; set; } = "";
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; } = "";
            [Display(Name = "Kön")]
            public string Gender { get; set; } 
            [Display(Name = "HCP")]
            public double HCP { get; set; }
            [Display(Name = "Golfid")]
            public int GolfID { get; set; } = 0;
            [Display(Name = "Medlemskategori")]
            public int Membercategory { get; set; } = 0;
            [Display(Name = "Telefonnummer")]
            public string Telephone { get; set; } = "";
            [Display(Name = "Betalat medlemsavgift")]
            public bool Payment { get; set; } = false;
        }
      
        
            public int ID { get; set; } = 0;
            [Display(Name = "Förnamn")]
            public string Firstname { get; set; } = "";
            [Display(Name = "Efternamn")]
            public string Lastname { get; set; } = "";
            [Display(Name = "Address")]
            public string Address { get; set; } = "";
            [Display(Name = "Postkod")]
            public string Postalcode { get; set; } = "";
            [Display(Name = "Ort")]
            public string City { get; set; } = "";
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; } = "";
            [Display(Name = "Email")]
            public int Gender { get; set; } = 0;
            [Display(Name = "Email")]
            public double HCP { get; set; } = 0.0;
            [Display(Name = "Email")]
            public string GolfID { get; set; } = "";
            [Display(Name = "Email")]
            public int Membercategory { get; set; } = 0;
            public string Telephone { get; set; } = "";
            public bool Payment { get; set; } = false;
        
    }
}