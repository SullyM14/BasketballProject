using BasketballBusinessLayer;
using BasketballProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasketballFrontEnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CRUD _crud = new CRUD();

        public MainWindow()
        {
            InitializeComponent();
            _crud.SelectedUser = new Users { UserId = 1 };
            PopulateUserTeamChoices();
        }

        private void PopulateUserTeamComboBox()
        {
            cmbUserTeams.ItemsSource = _crud.AllUserTeams();
        }

        private void PopulateUserTeamChoices()
        {
            ListBoxSelectTeams.ItemsSource = _crud.AllUserTeams();
        }

        private void PopulateUserTeam()
        {
            object team = _crud.SelectedUserTeam;
            cmbUserTeams.Visibility = Visibility.Visible;
            PopulateUserTeamComboBox();
            ListBoxUserTeams.ItemsSource = _crud.RetrieveUserTeamsPlayers(team);
            TextBudget.Text = _crud.SelectedUserTeam.Budget.ToString();
            RemoveTeamButton.Visibility = Visibility.Visible;
        }

        private void PopulateListNbaTeams()
        {
            ListBoxNbaTeams.ItemsSource = _crud.RetrieveNbaTeams();
        }

        private void ListBoxNbaTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListBoxNbaTeams.SelectedItem != null)
            {
                ListBoxNbaPlayers.ItemsSource = _crud.RetrieveTeamPlayers(ListBoxNbaTeams.SelectedItem);
            }
        }

        private void ListBoxNbaPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxNbaPlayers.SelectedItem != null)
            {
                _crud.SetSelectedPlayer(ListBoxNbaPlayers.SelectedItem);
                MakePlayerFieldsVisible();
                RemoveOrAddPlayerVisibleCheck(ListBoxNbaPlayers.SelectedItem);
                PopulatePlayerFields();
            }
        }

        private void PopulatePlayerFields()
        {
            if (_crud.SelectedPlayers != null) {
                TextPlayerName.Text = _crud.SelectedPlayers.ToString();
                TextPPG.Text = _crud.SelectedPlayers.Ppg.ToString();
                TextAPG.Text = _crud.SelectedPlayers.Apg.ToString();
                TextRPG.Text = _crud.SelectedPlayers.Rpg.ToString();
                TextPrice.Text = _crud.SelectedPlayers.Price.ToString();
            }
        }

        public void RemoveOrAddPlayerVisibleCheck(object selectedItem) 
        {
            //Check if player is already in team
            if (_crud.IsPlayerInTeam(selectedItem))
            {
                RemoveButton.Visibility = Visibility.Visible;
                AddPlayerButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                AddPlayerButton.Visibility = Visibility.Visible;
                RemoveButton.Visibility = Visibility.Collapsed;
            }
        }
        private void MakePlayerFieldsVisible()
        {
            //Collapse the user team fields
            UserTeamDetails.Visibility = Visibility.Collapsed;
            UserSelectTeams.Visibility = Visibility.Collapsed;
            //Make player details fields visible
            PlayerDetails.Visibility = Visibility.Visible;
        }

        private void ListBoxUserTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxUserTeams.SelectedItem != null)
            {
                _crud.SetSelectedPlayer(ListBoxUserTeams.SelectedItem);
                MakePlayerFieldsVisible();
                RemoveOrAddPlayerVisibleCheck(ListBoxUserTeams.SelectedItem);
                PopulatePlayerFields();
            }
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxNbaPlayers.SelectedItem != null)
            {
                _crud.AddPlayerToUserTeam(ListBoxNbaPlayers.SelectedItem);
                MakePlayerFieldsVisible();
                RemoveOrAddPlayerVisibleCheck(ListBoxNbaPlayers.SelectedItem);
                PopulatePlayerFields();
            }
        }

        private void MyTeamButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateUserTeam();
            MakeMyTeamFieldsVisible();
        }

        private void MakeMyTeamFieldsVisible()
        {
            //Collapse player details fields
            PlayerDetails.Visibility = Visibility.Collapsed;
            //Make the user team details fields visible
            UserTeamDetails.Visibility = Visibility.Visible;

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxNbaPlayers.SelectedItem != null)
            {
                _crud.RemovePlayerFromTeam(ListBoxNbaPlayers.SelectedItem);
                MakeMyTeamFieldsVisible();
                PopulateUserTeam();
            }
            else {
                if(ListBoxUserTeams.SelectedItem != null)
                {
                    _crud.RemovePlayerFromTeam(ListBoxUserTeams.SelectedItem);
                    MakeMyTeamFieldsVisible();
                    PopulateUserTeam();
                }
             }
        }

        private void ListBoxSelectTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListBoxSelectTeams.SelectedItem != null)
            {
                PopulateListNbaTeams();
                _crud.setSelectedUserTeam(ListBoxSelectTeams.SelectedItem);
                PopulateUserTeam();
                ListBoxSelectTeams.Visibility = Visibility.Collapsed;
                MakeMyTeamFieldsVisible();
            }
        }

        private void NewTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var newTeam = _crud.MakeNewUserTeam();
            ListBoxUserTeams.ItemsSource = _crud.RetrieveUserTeamsPlayers(newTeam);
            ListBoxSelectTeams.Visibility = Visibility.Collapsed;
            TextBudget.Text = _crud.SelectedUserTeam.Budget.ToString();
            PopulateListNbaTeams();
            MakeMyTeamFieldsVisible();
            MessageBox.Show("New Team Added successfully", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cmbUserTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUserTeams.SelectedItem != null)
            {
                PopulateListNbaTeams();
                _crud.setSelectedUserTeam(cmbUserTeams.SelectedItem);
                PopulateUserTeam();
                ListBoxSelectTeams.Visibility = Visibility.Collapsed;
                MakeMyTeamFieldsVisible();
            }
        }

        private void RemoveTeamButton_Click(object sender, RoutedEventArgs e)
        {
                _crud.RemoveUserTeam();
                _crud.SelectedUserTeam = null;
                MakeUserTeamsChoiceVisible();
        }

        private void MakeUserTeamsChoiceVisible()
        {
            //Populate and make visible Select teams
            PopulateUserTeamChoices();
            UserSelectTeams.Visibility = Visibility.Visible;
            ListBoxSelectTeams.Visibility = Visibility.Visible;
            //Collapse the rest
            PlayerDetails.Visibility = Visibility.Collapsed;
            UserTeamDetails.Visibility = Visibility.Collapsed;
        }
    }
}
