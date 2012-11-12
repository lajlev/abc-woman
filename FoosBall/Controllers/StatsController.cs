
namespace FoosBall.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Models;
    using FoosBall.Models.Views;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class StatsController : BaseController
    {
        public ActionResult Player(string playerId)
        {
            if (playerId != null)
            {
                var bestFriendsForever = new List<Player>();
                var realBestFriendsForever = new List<Player>();
                var evilArchEnemy = new List<Player>();
                var preferredColor = new List<string>();
                var winningColor = new List<string>();

                var id = ObjectId.Parse(playerId);
                var player = Dbh.GetCollection<Player>("Players").FindOne(Query.EQ("_id", id));
                var stats = new PlayerStatsViewModel {Player = player};

                var matches = Dbh.GetCollection<Match>("Matches")
                    .FindAll()
                    .SetSortOrder(SortBy.Descending())
                    .ToList()
                    .Where(match => match.ContainsPlayer(id))
                    .Where(match => match.GameOverTime != DateTime.MinValue);

                var playedMatches = matches as List<Match> ?? matches.ToList();
                stats.PlayedMatches = playedMatches.OrderByDescending(x => x.GameOverTime);

                foreach (var match in playedMatches)
                {
                    var teamMate = match.GetTeamMate(id);
                    if (teamMate != null)
                    {
                        bestFriendsForever.Add(teamMate);                        
                        if (match.WonTheMatch(id))
                        {
                            realBestFriendsForever.Add(teamMate);
                        }
                    }

                    if (match.IsOnRedTeam(id))
                    {
                        preferredColor.Add("red");
                        if (match.WonTheMatch(id))
                        {
                            winningColor.Add("red");
                        } 

                    }
                    else
                    {
                        preferredColor.Add("blue");
                        if (match.WonTheMatch(id))
                        {
                            winningColor.Add("blue");
                        }
                    }
                    
                    stats.Played++;
                    stats.Won = match.WonTheMatch(id) ? ++stats.Won : stats.Won;
                    stats.Lost = !match.WonTheMatch(id) ? ++stats.Lost : stats.Lost;
                    stats.PlayedToday = match.GameOverTime.ToLocalTime().Day == DateTime.Now.Day ? ++stats.PlayedToday : stats.PlayedToday;
                    stats.PlayedLast7Days = match.GameOverTime.ToLocalTime() > DateTime.Now.AddDays(-7)? ++stats.PlayedLast7Days : stats.PlayedLast7Days;
                    stats.PlayedLast30Days = match.GameOverTime.ToLocalTime() > DateTime.Now.AddDays(-30) ? ++stats.PlayedLast30Days : stats.PlayedLast30Days;
                }

                stats.BestFriendsForever = bestFriendsForever.Where(x => x.Name != null).GroupBy(s => s.Id).OrderByDescending(i => i.Count()).Select(x => x.First()).FirstOrDefault();
                stats.RealBestFriendsForever = realBestFriendsForever.Where(x => x.Name != null).GroupBy(s => s.Id).OrderByDescending(i => i.Count()).Select(x => x.First()).FirstOrDefault();
                stats.EvilArchEnemy = evilArchEnemy.GroupBy(s => s.Name).OrderByDescending(i => i.Count()).Select(x => x.First()).FirstOrDefault();
                var firstOrDefault = preferredColor.GroupBy(s => s).OrderByDescending(i => i.Count()).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    stats.PreferredColor = firstOrDefault.Key;
                }
                var orDefault = winningColor.GroupBy(s => s).OrderByDescending(i => i.Count()).FirstOrDefault();
                if (orDefault != null)
                {
                    stats.WinningColor = orDefault.Key;
                }
                stats.LatestMatch = playedMatches.Last();

                return View(stats);
            }

            return View();
        }

    }
}
