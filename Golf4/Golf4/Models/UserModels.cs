using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public void Generatepass(string ppassword, string user)
        {
            // Genererar en 192-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, 192))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(192);  // Skapar 192-byte key av lösenordet

                // Sparar salt och hash till databasen
                
                string salt1 = Encoding.Default.GetString(salt);
                string key1 = Encoding.Default.GetString(key);
                PostgresModels m = new PostgresModels();
                
                m.SqlNonQuery("update xxx set salt = @par2, hash = @par3 where user_name = @par1;", PostgresModels.list = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", user),
                    new NpgsqlParameter("@par2", salt1),
                    new NpgsqlParameter("@par3", key1)
                });


            }
        }
    }
}