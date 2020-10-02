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

        public void AddPlayerToUserTeam(object selectedItem)
        {
            using(var db = new BasketballProjectContext())
            {
                SetSelectedPlayer(selectedItem);

                var searchForPlayers=
                    from UserTeamPlayers in db.UserTeamPlayers
                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
                    select UserTeamPlayers;

                var isPlayerAlreadyInTeam = searchForPlayers.Count();

                if (isPlayerAlreadyInTeam != 1)
                {
                    db.Add(new UserTeamPlayers {UserTeamId = SelectedUserTeam.UserTeamId, PlayerId = SelectedPlayers.PlayerId });
                    db.SaveChanges();
                }
            }
        }

        public bool IsPlayerInTeam(object selectedItem)
        {
            using (var db = new BasketballProjectContext())
            {
                //Set the Selected Player
                SetSelectedPlayer(selectedItem);

                //Search to see if player is in the users team
                var searchForPlayers =
                    from UserTeamPlayers in db.UserTeamPlayers
                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
                    select UserTeamPlayers;

                var isPlayerAlreadyInTeam = searchForPlayers.Count();

                //if player is in team return true
                if (isPlayerAlreadyInTeam == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public void RemovePlayerFromTeam(object selectedItem)
        {
            using (var db = new BasketballProjectContext())
            {
                if (IsPlayerInTeam(selectedItem)) 
                {
                    var selectedPlayer =
                         from UserTeamPlayers in db.UserTeamPlayers
                         where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
                         select UserTeamPlayers;

                    db.UserTeamPlayers.RemoveRange(selectedPlayer);
                    db.SaveChanges();
                }
            }
        }
    }
}
