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
                _crudManager.SelectedUser = new Users { UserId = 1 };
                object selectedTeam = new UserTeams { UserTeamId = 1, UserId = 1 };
                _crudManager.setSelectedUserTeam(selectedTeam);
                object selectedItem = new Players { PlayerId = 1, FirstName = "Lebron", LastName = "James" };
                _crudManager.AddPlayerToUserTeam(selectedItem);
                object selectedItem2 = new Players { PlayerId = 10, FirstName = "Paul", LastName = "George" };
                _crudManager.AddPlayerToUserTeam(selectedItem2);
                object selectedItem3 = new Players { PlayerId = 3, FirstName = "Anthony", LastName = "Davis" };
                _crudManager.RemovePlayerFromTeam(selectedItem3);
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
        public void WhenStartedCorrectChoicesOfUserTeamsIsShown()
        {
            using (var db = new BasketballProjectContext())
            {
                var selectedUser = new Users { UserId = 1 };
                var userTeams =
                    from p in db.UserTeams
                    where p.UserId == selectedUser.UserId
                    select p;

                var numberOfTeams = userTeams.Count();

                var actual = _crudManager.AllUserTeams().Count();

                Assert.AreEqual(numberOfTeams, actual);
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

                object selectedItem = new UserTeams { UserId = 1, UserTeamId = 1 };
                var retrieveUserTeamPlayers = _crudManager.RetrieveUserTeamsPlayers(selectedItem).Count();

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
        public void WhenANewPlayerIsAddedToTheTeam_TheNumberOfPlayersInTheTeamIsIncreasedByOne()
        {
            using (var db = new BasketballProjectContext())
            {
                object selectedUserTeam = new UserTeams { UserId = 1, UserTeamId = 1 };
                var getPlayers = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersBefore = getPlayers.Count();
                object selectedItem = new Players { PlayerId = 3, FirstName = "Anthony", LastName = "Davis" };
                _crudManager.AddPlayerToUserTeam(selectedItem);
                var getPlayers2 = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersAfter = getPlayers2.Count();
                Assert.AreEqual(numberOfPlayersBefore + 1, numberOfPlayersAfter);
            }
        }

        [Test]
        public void WhenTryingToAddANewPlayerThatIsAlreadyinTeam_NumberOfPlayersDoesNotChange()
        {
            using (var db = new BasketballProjectContext())
            {
                object selectedUserTeam = new UserTeams { UserId = 1, UserTeamId = 1 };
                var getPlayers = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersBefore = getPlayers.Count();
                object selectedPlayer = new Players { PlayerId = 1, FirstName = "Lebron", LastName = "James" };
                _crudManager.AddPlayerToUserTeam(selectedPlayer);
                var getPlayers2 = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersAfter = getPlayers2.Count();
                Assert.AreEqual(numberOfPlayersBefore, numberOfPlayersAfter);
            }
        }

        [Test]
        public void WhenRemovingPlayerFromTeam_NumberOfPlayersInTeamDecreasesBy1()
        {
            using (var db = new BasketballProjectContext())
            {
                object selectedUserTeam = new UserTeams { UserId = 1, UserTeamId = 1 };
                var getPlayers = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersBefore = getPlayers.Count();
                object selectedPlayer = new Players { PlayerId = 10, FirstName = "Paul", LastName = "George" };
                _crudManager.RemovePlayerFromTeam(selectedPlayer);
                var getPlayers2 = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersAfter = getPlayers2.Count();
                Assert.AreEqual(numberOfPlayersBefore - 1, numberOfPlayersAfter);
            }
        }

        [Test]
        public void WhenRemovingPlayerFromTeamWhenThePlayerIsntInTheTeam_NumberOfPlayersInTeamStaysTheSame()
        {
            using (var db = new BasketballProjectContext())
            {
                object selectedUserTeam = new UserTeams { UserId = 1, UserTeamId = 1 };
                var getPlayers = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersBefore = getPlayers.Count();
                object selectedPlayer = new Players { PlayerId = 92, FirstName = "Cory", LastName = "Joseph" };
                _crudManager.RemovePlayerFromTeam(selectedPlayer);
                var getPlayers2 = _crudManager.RetrieveUserTeamsPlayers(selectedUserTeam);
                var numberOfPlayersAfter = getPlayers2.Count();
                Assert.AreEqual(numberOfPlayersBefore, numberOfPlayersAfter);
            }
        }

        [Test]
        public void WhenNewTeamAdded_NumberOfUserTeamsIncreasedByOne()
        {
            //_crudManager.MakeNewUserTeam();
            var UserTeamsBefore = _crudManager.AllUserTeams().Count();
            var newTeam = _crudManager.MakeNewUserTeam();
            var UserTeamsAfter = _crudManager.AllUserTeams().Count();
            Assert.AreEqual(UserTeamsBefore + 1, UserTeamsAfter);
        }

        [Test]
        public void WhenATeamIsRemoved_NumberOfTeamsDecreasesByOne()
        {
            var newTeam1 = _crudManager.MakeNewUserTeam();
            _crudManager.setSelectedUserTeam(newTeam1);
            var userTeamsBefore = _crudManager.AllUserTeams().Count();
            _crudManager.RemoveUserTeam();
            var userTeamsAfter = _crudManager.AllUserTeams().Count();

            Assert.AreEqual(userTeamsBefore - 1, userTeamsAfter);
        }

        [Test]
        public void WhenATeamIsRemovedThatDoesntExist_NumberOfTeamsStaysTheSame()
        {
            var newTeam = new UserTeams { UserId = 1, UserTeamId = 10 };
            _crudManager.setSelectedUserTeam(newTeam);
            var userTeamsBefore = _crudManager.AllUserTeams().Count();
            _crudManager.RemoveUserTeam();
            var userTeamsAfter = _crudManager.AllUserTeams().Count();

            Assert.AreEqual(userTeamsBefore, userTeamsAfter);
        }
    }
}