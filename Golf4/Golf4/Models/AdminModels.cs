using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
        public int Genderselected { get; set; }
        [Display(Name = "HCP")]
        public double HCP { get; set; }
        [Display(Name = "Golfid")]
        public string GolfID { get; set; }
        [Display(Name = "Medlemskategori")]
        public List<MembercategoryModels> Membercategory { get; set; }
        public int membercategoryselected { get; set; }
        [Display(Name = "Telefonnummer")]
        public string Telephone { get; set; }
        [Display(Name = "Betalat medlemsavgift")]
        public bool Payment { get; set; } = false;
        public string Password { get; set; }

        public List<MembercategoryModels> CollectMembercategory()
        {
            List<MembercategoryModels> list = new List<MembercategoryModels>();
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT * FROM membercategories", PostgresModels.list = new List<NpgsqlParameter>()
            {            
            });
            foreach (DataRow item in dt.Rows)
            {
                MembercategoryModels meber = new MembercategoryModels();
                meber.ID = (int)item["id"];
                meber.Category = (string)item["category"];
                list.Add(meber);
            }
            return list;
        }
        public List<GenderModels> CollectGender()
        {
            List<GenderModels> list = new List<GenderModels>();
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT * FROM genders", PostgresModels.list = new List<NpgsqlParameter>()
            {
            });
            foreach (DataRow item in dt.Rows)
            {
                GenderModels meber = new GenderModels();
                meber.id = (int)item["id"];
                meber.gender = (string)item["gender"];
                list.Add(meber);
            }
            return list;
        }
        public DataTable GetMember(int user_id)
        {
            PostgresModels sql = new PostgresModels();
            DataTable dt = sql.SqlQuery("SELECT members.id, members.firstname,members.lastname, members.address,members.postalcode,members.city,members.email,members.telephone,members.hcp,members.golfid,membercategory,gender  FROM members WHERE members.id = @par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", user_id)
            });
            return dt;
        }

        public void UpdateMemberAdmin(string fname, string lname, string address, string postalcode, string city, string email, string telephone, double hcp1, int gender, int membercategory, string golfid, bool payment, int id)
        {
            PostgresModels sql = new PostgresModels();
            sql.SqlNonQuery("UPDATE members SET firstname=@par2,lastname =@par3, address=@par4,postalcode=@par5,city=@par6,email=@par7,telephone=@par8,hcp=@par9 , gender=@par10 ,membercategory=@par11, golfid=@par12, payment=@par13 WHERE id =@par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par2", fname),
                new NpgsqlParameter("@par3", lname),
                new NpgsqlParameter("@par4", address),
                new NpgsqlParameter("@par5", postalcode),
                new NpgsqlParameter("@par6", city),
                new NpgsqlParameter("@par7", email),
                new NpgsqlParameter("@par8", telephone),
                new NpgsqlParameter("@par9", hcp1),
                new NpgsqlParameter("@par10", gender),
                new NpgsqlParameter("@par11", membercategory),
                new NpgsqlParameter("@par12", golfid),
                new NpgsqlParameter("@par13", payment),
                new NpgsqlParameter("@par1", id)
            });
        }
    }
    

    
}