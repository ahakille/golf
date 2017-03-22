using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace Golf4.Models
{
    public class MemberModels
    {
        public DataTable CollectOneMember(int user_id)
        {
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT members.id, members.firstname,members.lastname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategories.category,genders.gender  FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id where members.id = @par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", user_id)
            });
           
                return dt;
        }
        public DataTable CollectAllMembers()
        {
            PostgresModels sql = new PostgresModels();
            DataTable dt = new DataTable("data");
            dt = sql.SqlQuery("SELECT members.golfid AS \"GolfID\", members.firstname AS \"Förnamn\", members.lastname AS \"Efternamn\", members.hcp \"HCP\", membercategories.category AS \"Medlemskat.\", genders.gender AS \"Kön\", members.id FROM members LEFT JOIN membercategories ON members.membercategory = membercategories.id LEFT JOIN genders ON members.gender = genders.id", PostgresModels.list = new List<NpgsqlParameter>(){ });
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

        public static void CreateMember(MembersViewModel Member)
        {
            PostgresModels Database = new PostgresModels();

            DataTable table = Database.SqlQuery("SELECT id FROM membercategories WHERE category = @categoryname", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@@categoryname", Member.Membercategory),
            });

            int Membercategoryid = 0;

            foreach (DataRow Row in table.Rows)
            {
                Membercategoryid = (int)Row["id"];
                break;
            }
            
            Database.SqlNonQuery("INSERT INTO members (firstname, lastname, adress, postalcode, city, email, telephone, hcp, golfid, gender, membercategory, payment) VALUES (@firstname, @lastname, @adress, @postalcode, @city, @email, @telephone, @hcp, @golfid, @gender, @membercategory, @payment)", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@firstname", Member.Firstname),
                new NpgsqlParameter("@lastname", Member.Lastname),
                new NpgsqlParameter("@adress", Member.Adress),
                new NpgsqlParameter("@postalcode", Member.Postalcode),
                new NpgsqlParameter("@city", Member.City),
                new NpgsqlParameter("@email", Member.Email),
                new NpgsqlParameter("@telephone", Member.Telephone),
                new NpgsqlParameter("@hcp", Member.HCP),
                new NpgsqlParameter("@golfid", Member.GolfID),
                new NpgsqlParameter("@gender", Member.Gender),
                new NpgsqlParameter("@membercategory", Membercategoryid),
                new NpgsqlParameter("@payment", Member.Payment),

                new NpgsqlParameter("@payment", Member.Payment),
                new NpgsqlParameter("@payment", Member.Payment),
            });
        }

        public static void RemoveMember(int ID)
        {
            PostgresModels Database = new PostgresModels();
            Database.SqlNonQuery("DELETE FROM players WHERE memberid = @id; DELETE FROM balls WHERE userid = @id; DELETE FROM login WHERE userid = @id; DELETE FROM members WHERE id = @id;", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", ID)
            });
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
            public DataTable MyContests { get; set; }

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