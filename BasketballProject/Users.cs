using System;
using System.Collections.Generic;

namespace BasketballProject
{
    public partial class Users
    {
        public Users()
        {
            UserTeams = new HashSet<UserTeams>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserTeams> UserTeams { get; set; }
    }
}
