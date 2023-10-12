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
using Newtonsoft.Json;
using System.Net.Http;

namespace OODStarterCode_Feb20_2023
{
     /*
     * Additional feature is using materialdesign and my api call from multiple end points
     */

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //connect db
        NBAData db = new NBAData();

        //list of selected players
        protected List<Player> selectedPlayers = new List<Player>();

        //array of divisions
        string[] divisions = new string[] { "All", "Atlantic", "Central", "Southeast", "Northwest", "Pacific", "Southwest" };

        //declare random 
        Random rng = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ///add items to combobox
            cbxDivision.ItemsSource = divisions;

            //call method to display all teams
            DisplayAllTeams();
        }

        private void DisplayAllTeams()
        {
            //populate teams
            var query = from t in db.Teams
                        select t;

            //add all teams to listbox
            lbxTeams.ItemsSource = query.ToList();
            lbxTeams.Items.Refresh();
        }

        private void lbxTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //disable add player button
            btnAddPlayer.IsEnabled = false;

            //get selected team
            Team selectedTeam = lbxTeams.SelectedItem as Team;

            //populate players when team is selected
            if (selectedTeam != null)
            {
                var query = from p in db.Players
                            where p.TeamID == selectedTeam.Id
                            select p;
                lbxPlayers.ItemsSource = query.ToList();
            }
        }

        private void lbxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //enable add players button when player is selected
            if (lbxPlayers.SelectedItem != null)
            {
                btnAddPlayer.IsEnabled = true;
            }
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (lbxPlayers.SelectedItem != null)
            {
                //check if player is already added
                if (selectedPlayers.Contains(lbxPlayers.SelectedItem))
                {
                    MessageBox.Show("Player already selected choose another player");
                }
                else
                {
                    //add player to selected list
                    selectedPlayers.Add(lbxPlayers.SelectedItem as Player);
                    selectedPlayers.Sort();
                    lbxSelectedPlayer.ItemsSource = selectedPlayers;
                    lbxSelectedPlayer.Items.Refresh();
                }

            }
            else
            {
                MessageBox.Show("Please Select a Valid Player");
            }

        }

        private void btnAddRandomPlayer_Click(object sender, RoutedEventArgs e)
        {
            //list off all players
            List<Player> allPlayers = new List<Player>();

            //add all players from db to list
            allPlayers.AddRange(db.Players);

            //random player of type player
            Player randomPlayer = new Player();
            if (selectedPlayers.Count != allPlayers.Count)
            {
                //loop to generate random player till a player that isnt selected appears
                do
                {
                    int randomPlayerID = rng.Next(allPlayers.Count);
                    randomPlayer = allPlayers[randomPlayerID];
                } while (selectedPlayers.Contains(randomPlayer));

                //add player to selected players list
                selectedPlayers.Add(randomPlayer);

                //update selected player listbox
                lbxSelectedPlayer.ItemsSource = selectedPlayers;
                lbxSelectedPlayer.Items.Refresh();
            }
            else
            {
                //disable button once all players have been selected
                btnAddRandomPlayer.IsEnabled = false;
            }
        }

        private void cbxDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get selected division from combobox
            string selectedDivision = cbxDivision.SelectedItem as string;

            //filter division in teams table
            if (selectedDivision != null)
            {
                if (selectedDivision != "All")
                {
                    var query = from t in db.Teams
                                where t.Division == selectedDivision
                                select t;

                    //populate teams listbox
                    lbxTeams.ItemsSource = query.ToList();
                    lbxTeams.Items.Refresh();
                }
                else
                {
                    //if the option is all display all teams
                    DisplayAllTeams();
                }
            }
        }

        private void btnComparePlayers_Click(object sender, RoutedEventArgs e)
        {
            //when button is clicked show new window
            ComparePlayersWindow window = new ComparePlayersWindow();
            window.players = selectedPlayers;
            window.ShowDialog();

        }

        private void btnRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            //get selected player in selected players listbox
            Player selectedPlayer = lbxSelectedPlayer.SelectedItem as Player;

            //check if player is not null
            if (selectedPlayer != null)
            {
                //remove player from list and refresh listbox
                selectedPlayers.Remove(selectedPlayer);
                lbxSelectedPlayer.Items.Refresh();
            }
            else
            {
                //display error
                MessageBox.Show("Please select a player you want to remove from selected players list");
            }





        }
    }

}
