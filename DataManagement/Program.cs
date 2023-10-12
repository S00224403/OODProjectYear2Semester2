using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using OODStarterCode_Feb20_2023;
using System.Threading;

namespace DataManagement
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            NBAData db = new NBAData();
            using (db)
            {
                Console.WriteLine("Starting API calls");
                //get teams
                #region Teams
                //teams api call
                var client = new HttpClient();
                var response = await client.GetAsync("https://www.balldontlie.io/api/v1/teams?per_page=100");
                var json = await response.Content.ReadAsStringAsync();
                var teamData = JsonConvert.DeserializeObject<TeamData>(json);
                List<Team> teams = new List<Team>();//create list of teams
                Console.WriteLine("Getting teams");

                //create team object from teamdata object which holds json from api
                teams.AddRange(teamData.Data.Select(t => new Team
                {
                    Id = t.Id,
                    FullName = t.Full_Name,
                    Name = t.Name,
                    Abbreviation = t.Abbreviation,
                    City = t.City,
                    Conference = t.Conference,
                    Division = t.Division,
                    TeamImage = $"/images/{t.Name}.png"
                    
                }).ToList());
                Console.WriteLine("Teams created");
                
                #endregion Teams
                //player api call
                #region Players
                List<Player> players = new List<Player>();//create list of player objects
                client = new HttpClient();
                Console.WriteLine("Getting players");

                //api call for players which cycles through all player pages
                for (int i = 1; i < 53; i++)
                {
                    //api call
                    response = await client.GetAsync($"https://www.balldontlie.io/api/v1/players?per_page=100&page={i}");
                    json = await response.Content.ReadAsStringAsync();
                    var playerData = JsonConvert.DeserializeObject<PlayerData>(json);
                    //create player objects from json and add to player list
                    players.AddRange(playerData.Data.Select(p => new Player
                    {
                        ID = p.ID,
                        Name =$"{p.First_Name} {p.Last_Name}",
                        Height = $"{p.Height_Feet}'{p.Height_Inches}",
                        Position = p.Position,
                        TeamID = p.Team.ID,
                        Team = teams.Find(t => t.Id.Equals(p.Team.ID))
                    }));
                    players.Sort();
                }
                Console.WriteLine("Players created");
                
                #endregion Players
                //wait for 1 minute to reset api call limit
                Console.WriteLine("Waiting on API call limit reset");
                Thread.Sleep(60000);
                Console.WriteLine("Getting more data");

                #region Stats

                //Stats api call
                string stringSearch = "";
                List<Stats> stats = new List<Stats>();
                client = new HttpClient();
                Console.WriteLine("Getting statistics");
                //300 players per search
                for (int i = 300; i <= players.Count(); i += 300)
                {
                    //get string for api call with 300 player IDs
                    for (int j = i - 300; j < i; j++)
                    {
                        stringSearch += $"&player_ids[]={players[j].ID}";

                    }
                    //api call
                    response = await client.GetAsync($"https://www.balldontlie.io/api/v1/season_averages?season=2022" + stringSearch);
                    json = await response.Content.ReadAsStringAsync();
                    var statData = JsonConvert.DeserializeObject<StatsData>(json);
                    
                    //create stats object and add to stats list
                    stats.AddRange(statData.Data.Select(s => new Stats
                    {
                        PlayerID = s.Player_ID,
                        PPG = s.Pts,
                        Rebounds = s.Reb,
                        Assists = s.Ast,
                        GamesPlayed = s.Games_Played,
                        Minutes = s.Min,
                        Blocks = s.Blk,
                        Steals = s.Stl,
                        Fg3_Pct = s.Fg3_Pct,
                        Fg_Pct = s.Fg_Pct,
                        Ft_Pct = s.Ft_Pct,
                        Turnovers = s.Turnover,
                        Player = players.Find(p => p.ID.Equals(s.Player_ID))
                    }));
                    stringSearch = "";
                }
                Console.WriteLine("Stats created");
                
                //create list of valid player IDs
                List<int> validIds = new List<int>();
                //get all player ids in stats list
                foreach (var player in stats)
                {
                    validIds.Add(player.PlayerID);
                }
                //add players who played that season
                List<Player> validPlayers = players.Where(p => validIds.Contains(p.ID)).ToList()
                Console.WriteLine("Valid players sorted");
                #endregion Stats
                //Adding items to db
                foreach (var team in teams)
                {
                    if(team.Id <= 30)
                    {
                        db.Teams.Add(team);
                    }
                }
                Console.WriteLine("Added Teams");
                foreach (var playerStat in stats)
                {
                    db.Stats.Add(playerStat);
                }
                Console.WriteLine("Added Player Stats");
                foreach (var player in validPlayers)
                {
                    db.Players.Add(player);
                }
                Console.WriteLine("Added Players");
                db.SaveChanges();
                Console.WriteLine("Saved db");
            }
            Console.ReadLine();
        }
    }
}
