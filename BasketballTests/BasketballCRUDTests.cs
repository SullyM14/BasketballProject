using NUnit.Framework;
using BasketballBusinessLayer;
using BasketballProject;
using System.Linq;
using System.Data.Common;
using System;

namespace BasketballTests
{
    public class Tests
    {
        CRUD _crudManager;

        [SetUp]
        public void Setup()
        {
            _crudManager = new CRUD();
            using (var db = new BasketballProjectContext())
           {
                object selectedTeam = new UserTeams { UserTeamId = 1, UserId = 1 };
                _crudManager.setSelectedUserTeam(selectedTeam);
                object selectedItem = new Players { PlayerId = 1, FirstName = "Lebron", LastName = "James" };
                _crudManager.AddPlayerToUserTeam(selectedItem);
                //Need to Remove Anthony Davis if he exists using remove method in CRUDManager, 
            }
        }

        [Test]
        public void WhenAnNBATeamIsSelected_AllPlayersForThatTeamAreRetrieved()
        {
            using (var db = new BasketballProjectContext()) {
                var numberOfPlayersForLakers =
                    from p in db.Players
                    where p.TeamId == 2
                    select p;

                var selectedTeam = new Nbateams { NbateamId = 2 };
                var retrievePlayersRun = _crudManager.RetrieveTeamPlayers(selectedTeam).Count();
                var countExpectedPlayers = numberOfPlayersForLakers.Count();

                Assert.AreEqual(countExpectedPlayers, retrievePlayersRun);

            }
        }

        [Test]
        public void WhenAFanatasyTeamIsRetrieved_CorrectNumberOfPLayerShown()
        {
            using (var db = new BasketballProjectContext())
            {
                var selectedUserTeam = new UserTeams { UserId = 1, UserTeamId = 1 };
                var playersInTeam =
                    from p in db.UserTeamPlayers
                    where p.UserTeamId == selectedUserTeam.UserTeamId
                    select p;

                var retrieveUserTeamPlayers = _crudManager.RetrieveUserTeams().Count();

                Assert.AreEqual(playersInTeam.Count(), retrieveUserTeamPlayers);

            }
        }

        [Test]
        public void WhenAskedForNBATeams_AllNBATeamsAreShown()
        {
            using (var db = new BasketballProjectContext())
            {
                var numberOfTeams = db.Nbateams.Count();
                var retrieveNBATeams = _crudManager.RetrieveNbaTeams().Count();
                Assert.AreEqual(numberOfTeams, retrieveNBATeams);

            }
        }

        [Test]
        public void WhenSelectedPlayer_CorrectDetailsSelected()
        {
            using (var db = new BasketballProjectContext())
            {
                var selectedPlayer = new Players { PlayerId = 1, FirstName = "Lebron", LastName = "James" };
                var player =
                    from p in db.Players
                    where p.PlayerId == selectedPlayer.PlayerId
                    select p;

                var playerName = player.FirstOrDefault().FirstName;

                _crudManager.SetSelectedPlayer(selectedPlayer);
                var name = _crudManager.SelectedPlayers.FirstName;

                Assert.AreEqual(playerName, name);
            }

        }
   
        [Test]
        //[Ignore("Feature not completely Implmented yet")]
        public void WhenANewPlayerIsAddedToTheTeam_TheNumberOfPlayersInTheTeamIsIncreasedByOne()
        {
            using(var db = new BasketballProjectContext())
            {
                var getPlayers = _crudManager.RetrieveUserTeams();
                var numberOfPlayersBefore = getPlayers.Count();
                object selectedItem = new Players { PlayerId = 3, FirstName = "Anthony", LastName = "Davis" };
                _crudManager.AddPlayerToUserTeam(selectedItem);
                var getPlayers2 = _crudManager.RetrieveUserTeams();
                var numberOfPlayersAfter = getPlayers2.Count();
                Assert.AreEqual(numberOfPlayersBefore + 1, numberOfPlayersAfter);
            }
        }

        [Test]
        public void WhenTryingToAddANewPlayerThatIsAlreadyinTeam_NumberOfPlayersDoesNotChange()
        {
            using (var db = new BasketballProjectContext())
            {
                var getPlayers = _crudManager.RetrieveUserTeams();
                var numberOfPlayersBefore = getPlayers.Count();
                object selectedPlayer = new Players { PlayerId = 1, FirstName = "Lebron", LastName = "James" };
                _crudManager.AddPlayerToUserTeam(selectedPlayer);
                var getPlayers2 = _crudManager.RetrieveUserTeams();
                var numberOfPlayersAfter = getPlayers2.Count();
                Assert.AreEqual(numberOfPlayersBefore, numberOfPlayersAfter);
            }
        }
    }
}