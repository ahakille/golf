using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class MemberModels
    {
        public int ID { get; set; } = 0;
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; } = "";
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; } = "";
        [Display(Name = "Adress")]
        public string Address { get; set; } = "";
        [Display(Name = "Postkod")]
        public string Postalcode { get; set; } = "";
        [Display(Name = "Ort")]
        public string City { get; set; } = "";
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; } = "";
        public int Gender { get; set; } = 0;
        public double HCP { get; set; } = 0.0;
        public string GolfID { get; set; } = "";
        public int Membercategory { get; set; } = 0;
        public string Telephone { get; set; } = "";
        public bool Payment { get; set; } = false;
        public ReservationModels Reservation { get; set; } = new ReservationModels();
    }
}