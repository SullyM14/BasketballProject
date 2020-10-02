using System;
using System.Collections.Generic;

namespace BasketballProject
{
    public partial class Nbateams
    {
        public Nbateams()
        {
            Players = new HashSet<Players>();
        }

        public int NbateamId { get; set; }
        public string TeamName { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public string Conference { get; set; }

        public virtual ICollection<Players> Players { get; set; }
    }
}
