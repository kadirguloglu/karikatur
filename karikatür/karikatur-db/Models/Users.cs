using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Users
    {
        public Guid Id { get; set; }
        public Guid Password { get; set; }
        public string Email { get; set; }
        public DateTime PasswordExpirationDate { get; set; }
    }
}
