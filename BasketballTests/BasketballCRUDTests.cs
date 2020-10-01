using NUnit.Framework;
using BasketballBusinessLayer;
using BasketballProject;
using System.Linq;
using System.Data.Common;

namespace BasketballTests
{
    public class Tests
    {
        CRUD _crudManager;

        [SetUp]
        public void Setup()
        {
            _crudManager = new CRUD();
        }

        [Test]
        public void WhenAnNBATeamIsSelected_AllPlayersForThatTeamAreRetrieved()
        {
            using (var db = new BasketballProjectContext()) {
                var numberOfPlayersForLakers =
                    from p in db.Players
                    where p.TeamId == 2
                    select p;

                var selectedTeam = new Nbateams{NbateamId = 2};
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
            using(var db = new BasketballProjectContext())
            {
                var numberOfTeams = db.Nbateams.Count();
                var retrieveNBATeams = _crudManager.RetrieveNbaTeams().Count();
                Assert.AreEqual(numberOfTeams, retrieveNBATeams);

            }
        }

        [Test]
        public void WhenSelectedPlayer_CorrectDetailsSelected()
        {
            using(var db = new BasketballProjectContext())
            {
                var selectedPlayer = new Players {PlayerId = 1, FirstName = "Stephen", LastName = "Curry"};
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
    }
}