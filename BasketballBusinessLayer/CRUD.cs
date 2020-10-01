using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasketballProject;
using Microsoft.EntityFrameworkCore;

namespace BasketballBusinessLayer
{
    public class CRUD
    {
        public UserTeams SelectedUserTeam { get; set; }
        public Nbateams SelectedNbaTeam { get; set; }

        public Players SelectedPlayers { get; set; }

        public Users SelectedUser { get; set; }

        public List<Nbateams> RetrieveNbaTeams()
        {
            using (var db = new BasketballProjectContext())
            {
                return db.Nbateams.ToList();
            }
        }

        public void setSelectedNbaTeam(object selectedItem)
        {
            SelectedNbaTeam = (Nbateams)selectedItem;
        }

        public void setSelectedUserTeam(object selectedItem)
        {
            SelectedUserTeam = (UserTeams)selectedItem;
        }

        public List<Players> RetrieveTeamPlayers(object selectedItem)
        {
            using (var db = new BasketballProjectContext())
            {
                SelectedNbaTeam = (Nbateams)selectedItem;
                var players =
                    from p in db.Players
                    where p.TeamId == SelectedNbaTeam.NbateamId
                    select p;

                return players.ToList();
            }
        }

        public List<Players> RetrieveUserTeams()
        {
            using(var db = new BasketballProjectContext())
            {
                var users =
                    from u in db.Users
                    where u.UserId == 1
                    select u;

                SelectedUser = users.FirstOrDefault();

                var userTeam =
                    from uTeam in db.UserTeams
                    where uTeam.UserId == SelectedUser.UserId
                    select uTeam;

                SelectedUserTeam = userTeam.FirstOrDefault();


                var fantasyPlayers =
                     from uTeamPlayers in db.UserTeamPlayers.Include(ut => ut.UserTeam).Include(p => p.Player)
                     where (uTeamPlayers.UserTeam.UserId == SelectedUser.UserId) && (uTeamPlayers.UserTeamId == uTeamPlayers.UserTeam.UserTeamId)
                     select uTeamPlayers.Player;


                return fantasyPlayers.ToList();
            }
        }

        public void SetSelectedPlayer(object selectedItem)
        {
            SelectedPlayers = (Players)selectedItem;
        }
    }
}
