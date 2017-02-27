using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf4.Models
{
    public class Member
    {
        public int ID { get; set; } = 0;
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Adress { get; set; } = "";
        public string Postalcode { get; set; } = "";
        public string City { get; set; } = "";
        public string Email { get; set; } = "";
        public Gender Gender { get; set; }
        public double HCP { get; set; } = 0.0;
        public int GolfID { get; set; } = 0;
        public Membercategory Membercategory { get; set; } = new Membercategory();
        public string Telefone { get; set; } = "";
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,        
    }
}