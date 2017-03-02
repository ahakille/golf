using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Golf4.Models
{
    public class UserModels
    {
        public class LoginViewModel
        {
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public class NewuserViewModel
        {
            [Required]
            [Display(Name = "Userid")]
            public int Userid { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        /// <summary>
        /// Skapar ett lösenordshash och salt, retunerar salt och hash
        /// </summary>
        /// <param name="ppassword"></param>
        /// <returns></returns>
        public Tuple<byte[],byte[]> Generatepass(string ppassword)
        {
            // Genererar en 192-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, 20))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(20);  // Skapar 100-byte key av lösenordet

                return Tuple.Create(salt, key);

            }
        }
        /// <summary>
        ///  kontrollerar lösenord mot databasen, Genererar ett True och false
        /// </summary>
        /// <param name="ppassword"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool AuthenticationUser(string ppassword, string userid)
        {
            
            byte[] salt = null, key =null;
            PostgresModels m = new PostgresModels();

            var dt = m.SqlQuery("select salt, key from login where userid =@par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", Convert.ToInt16(userid)),

            });
            foreach (DataRow dr in dt.Rows)
            {
                salt = (byte[])dr["salt"];
                key = (byte[])dr["key"];
            }
                //salt = Encoding.UTF8.GetBytes(ssalt);
                //key = Encoding.UTF8.GetBytes(skey);

                using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, salt))
                {
                    byte[] newKey = deriveBytes.GetBytes(20);
                    if (!newKey.SequenceEqual(key))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            
        }
    }
}