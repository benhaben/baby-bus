using System;
using System.ComponentModel.DataAnnotations;

namespace AdministratorManagement.Models
{
    public class UserLogin
    {
        public UserLogin()
        {
            Id = Guid.NewGuid();
        }

        public UserLogin(string p1, string p2, string p3, string[] p4)
        {
            //_userRepository.Save(new UserLogin("baibai", Convert.ToBase64String(saltedPass), Convert.ToBase64String(salt), new[] { Roles.Administrator }));

            Id = Guid.NewGuid();
            this.Username = p1;
            this.PasswordHash = p2;
            this.Salt = p3;
            this.Roles = p4;
        }
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }

        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string[] Roles { get; set; }
    }
}