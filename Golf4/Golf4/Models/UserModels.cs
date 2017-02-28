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
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

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
        public Tuple<string,string> Generatepass(string ppassword)
        {
            // Genererar en 192-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, 192))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(192);  // Skapar 192-byte key av lösenordet

                // Sparar salt och hash till databasen
                
                string salt1 = Encoding.Default.GetString(salt);
                string key1 = Encoding.Default.GetString(key);
                //PostgresModels m = new PostgresModels();
                
                //m.SqlNonQuery("update xxx set salt = @par2, hash = @par3 where user_name = @par1;", PostgresModels.list = new List<NpgsqlParameter>()
                //{
                //    new NpgsqlParameter("@par1", user),
                //    new NpgsqlParameter("@par2", salt1),
                //    new NpgsqlParameter("@par3", key1)
                //});
                return Tuple.Create(salt1, key1);

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
            string ssalt = "", skey = "";
            byte[] salt, key;
            PostgresModels m = new PostgresModels();

            var dt = m.SqlQuery("select salt, hash from login where userid =@par1", PostgresModels.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", userid),

            });
            foreach (DataRow dr in dt.Rows)
            {
                ssalt = dr["salt"].ToString();
                skey = dr["password"].ToString();
            }
                salt = Encoding.Default.GetBytes(ssalt);
                key = Encoding.Default.GetBytes(skey);

                using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, salt))
                {
                    byte[] newKey = deriveBytes.GetBytes(100);
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