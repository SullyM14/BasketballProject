﻿using System;
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

        public List<UserTeams> AllUserTeams()
        {
            using (var db = new BasketballProjectContext())
            {
                return db.UserTeams.ToList();
            }
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

        public List<Players> RetrieveUserTeamsPlayers(object selectedItem)
        {
            using(var db = new BasketballProjectContext())
            {
                var users =
                    from u in db.Users
                    where u.UserId == SelectedUser.UserId
                    select u;

                SelectedUser = users.FirstOrDefault();

                setSelectedUserTeam(selectedItem);

                var fantasyPlayers =
                     from uTeamPlayers in db.UserTeamPlayers.Include(ut => ut.UserTeam).Include(p => p.Player)
                     where (uTeamPlayers.UserTeam.UserId == SelectedUser.UserId) && (uTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId)
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

        public object MakeNewUserTeam()
        {
            using(var db = new BasketballProjectContext())
            {
                var users =
                    from u in db.Users
                    where u.UserId == SelectedUser.UserId
                    select u;

                SelectedUser = users.FirstOrDefault();
                var userTeam = new UserTeams { UserId = SelectedUser.UserId };
                db.UserTeams.Add(userTeam);
                db.SaveChanges();
                db.Entry(userTeam).GetDatabaseValues();
                object newTeam = db.Entry(userTeam).Entity;
                setSelectedUserTeam(newTeam);
                return newTeam;
            }
        }

        public void RemoveUserTeam()
        {
            using (var db = new BasketballProjectContext())
            {
                var searchForPlayers =
                    from UserTeamPlayers in db.UserTeamPlayers
                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId)
                    select UserTeamPlayers;

                var isPlayersInUserTeam = searchForPlayers.Count();

                if(isPlayersInUserTeam > 0)
                {
                    db.UserTeamPlayers.RemoveRange(searchForPlayers);
                    db.SaveChanges();
                }

                var userTeam =
                    from ut in db.UserTeams
                    where ut.UserTeamId == SelectedUserTeam.UserTeamId
                    select ut;

                db.UserTeams.RemoveRange(userTeam);
                db.SaveChanges();

               // throw new NotImplementedException();
            }
            }
    }
}
