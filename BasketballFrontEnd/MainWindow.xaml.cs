using BasketballBusinessLayer;
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
            PopulateListNbaTeams();
            PopulateUserTeam();
        }

        private void PopulateUserTeam()
        {
            ListBoxUserTeams.ItemsSource = _crud.RetrieveUserTeams();

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
            }
        }

        private void MakePlayerFieldsVisible()
        {
            //Collapse the user team fields
            ListBoxUserTeams.Visibility = Visibility.Collapsed;
            TitleMyTeam.Visibility = Visibility.Collapsed;
            //Make player details fields visible
            MyTeamButton.Visibility = Visibility.Visible;
            TextPlayerName.Visibility = Visibility.Visible;
            TextPPG.Visibility = Visibility.Visible;
            TextAPG.Visibility = Visibility.Visible;
            TextRPG.Visibility = Visibility.Visible;
            ReboundsLabel.Visibility = Visibility.Visible;
            AssistsLabel.Visibility = Visibility.Visible;
            PointsLabel.Visibility = Visibility.Visible;
            AddPlayerButton.Visibility = Visibility.Visible;
        }

        private void ListBoxUserTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxUserTeams.ItemsSource = _crud.RetrieveUserTeams();
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxNbaPlayers.SelectedItem != null)
            {
                _crud.AddPlayerToUserTeam(ListBoxNbaPlayers.SelectedItem);
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
            MyTeamButton.Visibility = Visibility.Collapsed;
            TextPlayerName.Visibility = Visibility.Collapsed;
            TextPPG.Visibility = Visibility.Collapsed;
            TextAPG.Visibility = Visibility.Collapsed;
            TextRPG.Visibility = Visibility.Collapsed;
            ReboundsLabel.Visibility = Visibility.Collapsed;
            AssistsLabel.Visibility = Visibility.Collapsed;
            PointsLabel.Visibility = Visibility.Collapsed;
            AddPlayerButton.Visibility = Visibility.Collapsed;
            //Make the user team fields visible
            ListBoxUserTeams.Visibility = Visibility.Visible;
            TitleMyTeam.Visibility = Visibility.Visible;
        }
    }
}
