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
using System.Windows.Shapes;

namespace OODStarterCode_Feb20_2023
{
    /// <summary>
    /// Interaction logic for ComparePlayersWindow.xaml
    /// </summary>
    public partial class ComparePlayersWindow : Window
    {
        NBAData db = new NBAData();
        public List<Player> players = new List<Player>();
        public ComparePlayersWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<Stats> playerStats = new List<Stats>();
            try
            {
                //get stats foreach player
                foreach (var player in players)
                {
                    var query = from s in db.Stats
                                where s.PlayerID == player.ID
                                select s;

                    //add stats to list
                    playerStats.AddRange(query.ToList());
                }
                //query to get data for datagrid using anonymous type
                var displayStats = from s in playerStats
                                   select new
                                   {
                                       Name = s.Player.Name,
                                       Team = s.Player.Team.FullName,
                                       Position = s.Player.Position,
                                       MinutesPlayed = s.Minutes,
                                       PPG = s.PPG,
                                       Rebounds = s.Rebounds,
                                       Assists = s.Assists,
                                       Steals = s.Steals,
                                       Blocks = s.Blocks,
                                       Turnovers = s.Turnovers,
                                       FieldGoalPercentage = s.Fg_Pct,
                                       ThreePointPercentage = s.Fg3_Pct,
                                       FreeThrowPercentage = s.Ft_Pct
                                   };
                //assign items to datagrid
                dgPlayers.ItemsSource = displayStats;
                
            }
            catch (Exception)
            {
                //show error if no players are selected
                MessageBox.Show("Please have players selected");
                this.Close();
            }
        }




    }
}
