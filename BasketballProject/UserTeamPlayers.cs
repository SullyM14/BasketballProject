using System;
using System.Collections.Generic;

namespace BasketballProject
{
    public partial class UserTeamPlayers
    {
        public int Id { get; set; }
        public int UserTeamId { get; set; }
        public int PlayerId { get; set; }

        public virtual Players Player { get; set; }
        public virtual UserTeams UserTeam { get; set; }
    }
}
