using System;
using System.Collections.Generic;

namespace BasketballProject
{
    public partial class Players
    {
        public Players()
        {
            UserTeamPlayers = new HashSet<UserTeamPlayers>();
        }

        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal Ppg { get; set; }
        public decimal Apg { get; set; }
        public decimal Rpg { get; set; }
        public int? Points { get; set; }
        public int? Rebounds { get; set; }
        public int? Assists { get; set; }
        public int TeamId { get; set; }
        public int? GamesPlayed { get; set; }

        public virtual Nbateams Team { get; set; }
        public virtual ICollection<UserTeamPlayers> UserTeamPlayers { get; set; }
    }
}
