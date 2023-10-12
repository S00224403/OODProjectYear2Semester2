using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace OODStarterCode_Feb20_2023
{
    #region PlayerClasses
    /// <summary>
    /// List of API Data
    /// </summary>
    public class PlayerData
    {
        public List<PlayerObject> Data { get; set; }
    }
    /// <summary>
    /// player class for db
    /// </summary>
    public class Player : IComparable<Player>
    {
        string height;
        string position;
        public int ID { get; set; }
        public string Name { get; set; }
        
        public string Height
        {
            get
            {
                return (this.height == "'" ? "N/A" : this.height);
            }
            set
            {
                this.height = (value == null ? "N/A" : value);
            }
        }
        
        public string Position
        {
            get
            {
                return (this.position == "" ? "N/A" : this.position);
            }
            set
            {
                this.position = (value == null ? "N/A" : value);
            }
        }
        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        public int CompareTo(Player other)
        {
            string[] thisNameParts = this.Name.Split(' ');
            string[] otherNameParts = other.Name.Split(' ');

            string thisLastName = thisNameParts.LastOrDefault();
            string otherLastName = otherNameParts.LastOrDefault();

            return thisLastName.CompareTo(otherLastName);
        }
    }
    /// <summary>
    /// Class to match api data names
    /// </summary>
    public class PlayerObject
    {
        public int ID { get; set; }
        public string First_Name { get; set; }
        public string Height_Feet { get; set; }
        public string Height_Inches { get; set; }
        public string Last_Name { get; set; }
        public string Position { get; set; }
        public TeamArray Team { get; set; } // change the type of the Team property to the TeamObject class
        
    }
    /// <summary>
    /// Class to get into nested json in player api call
    /// </summary>
    public class TeamArray
    {
        public int ID {get; set; }
    }
    #endregion PlayerClasses
    #region StatsClasses
    /// <summary>
    /// Class to match api data names
    /// </summary>
    public class StatsObject
    {
        public int Games_Played { get; set; }
        public int Player_ID { get; set; }
        public string Min { get; set; }
        public double Pts { get; set; }
        public double Reb { get; set; }
        public double Ast { get; set; }
        public double Stl { get; set; }
        public double Blk { get; set; }
        public double Turnover { get; set; }
        public double Fg_Pct { get; set; }
        public double Fg3_Pct { get; set; }
        public double Ft_Pct { get; set; }
    }
    /// <summary>
    /// stats class for db
    /// </summary>
    public class Stats
    {
        public int ID { get; set; }
        public int GamesPlayed { get; set; }
        public int PlayerID { get; set; }
        public string Minutes { get; set; }
        public double PPG { get; set; }
        public double Rebounds { get; set; }
        public double Assists { get; set; }
        public double Steals { get; set; }
        public double Blocks { get; set; }
        public double Turnovers { get; set; }
        public double Fg_Pct { get; set; }
        public double Fg3_Pct { get; set; }
        public double Ft_Pct { get; set; }
        public virtual Player Player { get; set; }
    }
    /// <summary>
    /// List of API Data
    /// </summary>
    public class StatsData
    {
        public List<StatsObject> Data { get; set; }
    }
    #endregion StatsClasses
    #region TeamClasses
    /// <summary>
    /// List of API Data
    /// </summary>
    public class TeamData
    {
        public List<TeamObject> Data { get; set; }
    }
    /// <summary>
    /// class to match json in teams api call
    /// </summary>
    public class TeamObject
    {
        public int Id { get; set; }
        public string Full_Name { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string City { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
    }
    /// <summary>
    /// team class for db
    /// </summary>
    public class Team
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string City { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string TeamImage { get; set; }
        public virtual List<Player> Players { get; set; }

    }
    #endregion TeamClasses
    /// <summary>
    /// db class
    /// </summary>
    public class NBAData: DbContext
    {
        public NBAData() : base("MyNBAData17Apr"){}
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Stats> Stats { get; set; }

    }
}
