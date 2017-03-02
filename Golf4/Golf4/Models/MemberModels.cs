using System.ComponentModel.DataAnnotations;

namespace Golf4.Models
{
    public class MemberModels
    {
        public class MembersViewModel
        {
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
            [Display(Name = "")]
            public int Gender { get; set; } 
            [Display(Name = "HCP")]
            public double HCP { get; set; }
            
        }
      
        
            public int ID { get; set; } = 0;
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
            [Display(Name = "Email")]
            public int Gender { get; set; } = 0;
            [Display(Name = "Email")]
            public double HCP { get; set; } = 0.0;
            [Display(Name = "Email")]
            public int GolfID { get; set; } = 0;
            [Display(Name = "Email")]
            public int Membercategory { get; set; } = 0;
            public string Telephone { get; set; } = "";
            public bool Payment { get; set; } = false;
        
    }

}