using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public void ResetBudget()
        {
            using (var db = new BasketballProjectContext())
            {
                SelectedUserTeam.Budget = 100;
                db.UserTeams.Update(SelectedUserTeam);
                db.SaveChanges();
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


                var numberOfPlayersInTeam = RetrieveUserTeamsPlayers(SelectedUserTeam).Count(); // Get all players in team
               
                //Find the user team in the database
                var userTeam1 =
                    from u in db.UserTeams
                    where u.UserTeamId == SelectedUserTeam.UserTeamId
                    select u;

                var userTeam = userTeam1.FirstOrDefault();

                //Find the players in the user team
                var searchForPlayers=
                    from UserTeamPlayers in db.UserTeamPlayers
                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
                    select UserTeamPlayers;

                //Check if the player is in the team
                var isPlayerAlreadyInTeam = searchForPlayers.Count();

                //Ensure the player has less than 6 players in their team and then the player is in budget
                if (isPlayerAlreadyInTeam != 1)
                {
                    if(numberOfPlayersInTeam < 6)
                        if (SelectedPlayers.Price <= userTeam.Budget)
                        {
                            //Update budget and then add the player
                            userTeam.Budget -= SelectedPlayers.Price;
                            db.UserTeams.Update(userTeam);                 
                            db.Add(new UserTeamPlayers { UserTeamId = SelectedUserTeam.UserTeamId, PlayerId = SelectedPlayers.PlayerId });
                            db.SaveChanges();
                            setSelectedUserTeam(userTeam);
                        }
                        else
                        {
                            throw new OutOfBudgetException();
                        }
                    else
                    {
                        throw new TooManyPlayerException();
                    }
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

        public void RemovePlayerFromTeam()
        {
            using (var db = new BasketballProjectContext())
            {
                //Check if the player is in the team, if they are then find the specfic player
                if (IsPlayerInTeam(SelectedPlayers)) 
                {
                    var selectedPlayer =
                         from UserTeamPlayers in db.UserTeamPlayers
                         where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId) && (UserTeamPlayers.PlayerId == SelectedPlayers.PlayerId)
                         select UserTeamPlayers;
                        
                    
                    //Update the budget and then remove the player
                    SelectedUserTeam.Budget += SelectedPlayers.Price;
                    db.UserTeams.Update(SelectedUserTeam);
                    db.SaveChanges();
                    db.UserTeamPlayers.RemoveRange(selectedPlayer);
                    db.SaveChanges();
                }
            }
        }

        public object MakeNewUserTeam(string text)
        {
            using(var db = new BasketballProjectContext())
            {
                //Set the User
                var users =
                    from u in db.Users
                    where u.UserId == SelectedUser.UserId
                    select u;

                SelectedUser = users.FirstOrDefault();
                
                //Add the user team and then update the selectedTeam to be the new team created
                var userTeam = new UserTeams { UserId = SelectedUser.UserId, Budget = 100, TeamName = text};
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
                //Find The Players in the team
                var searchForPlayers =
                    from UserTeamPlayers in db.UserTeamPlayers
                    where (UserTeamPlayers.UserTeamId == SelectedUserTeam.UserTeamId)
                    select UserTeamPlayers;

                //Check if the the player is in the userTeam
                var isPlayersInUserTeam = searchForPlayers.Count();

                //If they are in the team then remove them
                if(isPlayersInUserTeam > 0)
                {
                    db.UserTeamPlayers.RemoveRange(searchForPlayers);
                    db.SaveChanges();
                }

                //Then find the user team and remove the whole team
                var userTeam =
                    from ut in db.UserTeams
                    where ut.UserTeamId == SelectedUserTeam.UserTeamId
                    select ut;

                db.UserTeams.RemoveRange(userTeam);
                db.SaveChanges();
            }
            }
    }
}
