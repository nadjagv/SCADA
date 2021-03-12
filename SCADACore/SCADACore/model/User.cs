using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCADACore.model
{
    public enum UserRole { ADMIN, REGULAR}
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public User() { }
        public User(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = (UserRole)Enum.Parse(typeof(UserRole), role);
        }
    }
}