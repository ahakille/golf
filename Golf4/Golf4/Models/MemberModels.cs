using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class MemberModels
    {
        public DataTable CollectAllMembers()
        {
            PostgresModels sql = new PostgresModels();
            DataTable dt = new DataTable("data");
            dt = sql.SqlQuery("SELECT members.golfid , members.firstname,members.lastname,members.hcp,membercategories.category,genders.gender,members.id FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id", PostgresModels.list = new List<NpgsqlParameter>(){ });
            return dt;
        }
        public DataTable CollectContestWithMembers(int user_id)
        {
            PostgresModels sql = new PostgresModels();
            DataTable dt = new DataTable("data");
            dt = sql.SqlQuery("SELECT contests.name as \"Namn\", reservations.timestart as \"Startar\", reservations.timeend as \"Slutar\", contests.id as \"Välj åtgärd\" FROM reservations, contests LEFT JOIN players on contestid= contests.id WHERE memberid=@user_id AND reservations.id = contests.reservationid AND reservations.timestart > CURRENT_DATE AND contests.closetime > CURRENT_DATE", PostgresModels.list = new List<NpgsqlParameter>() {
                new NpgsqlParameter("@user_id", user_id)
            });

            return dt;
        }
        public List<MembersViewModel> GetUserInfo(int user_id)
        {
            PostgresModels sql = new PostgresModels();

            return null;
        }
        public DataTable GetBookingsOnMember(int user_id)
        {
            
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT reservations.id as \"Reservation\", reservations.timestart as \"Tillfälle\", reservations.id as \"Avboka\" FROM reservations JOIN balls ON balls.reservationid=reservations.id WHERE balls.userid=@identity AND DATE(reservations.timestart) >= current_date ORDER BY timestart", PostgresModels.list = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@identity",user_id),
             });
            return dt;
        }
        public class MembersViewModel
        {
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
            public string Gender { get; set; } 
            [Display(Name = "HCP")]
            public double HCP { get; set; }
            [Display(Name = "Golfid")]
            public string GolfID { get; set; } 
            [Display(Name = "Medlemskategori")]
            public string Membercategory { get; set; } 
            [Display(Name = "Telefonnummer")]
            public string Telephone { get; set; } 
            [Display(Name = "Betalat medlemsavgift")]
            public bool Payment { get; set; } = false;
            [Display(Name = "Datum och tid")]
            public DataTable Timestart { get; set; }
            public DateTime timestart { get; set; }
            public int ReservationID { get; set; }
            public string CheckedIn { get; set; }
            public DateTime TimestartTemp { get; set; }
            [Display(Name ="Tävling")]
            public DataTable CompeteList { get; set; }
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