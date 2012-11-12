
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
            // Hack: Because "old" Jakob was deleted by accident, we point "old" Jakob to "new" Jakob
            var id = (playerId == "508e36b90fa6810e90a3165c") ? ObjectId.Parse("50918252592eff0e9088b4df") : ObjectId.Parse(playerId); 

            if (playerId != null)
            {
                var bff = new Dictionary<BsonObjectId, BestFriendForever>();
                var rbff = new Dictionary<BsonObjectId, RealBestFriendForever>();
                var eae = new Dictionary<BsonObjectId, EvilArchEnemy>();
                var preferredColor = new Dictionary<string, PreferredColor>();
                var winningColor = new Dictionary<string, WinningColor>();
                const string Blue = "blue";
                const string Red = "red";

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
                    if (teamMate.Id != null)
                    {
                        if (bff.ContainsKey(teamMate.Id))
                        {
                            bff[teamMate.Id].Occurrences++;
                        }
                        else
                        {
                            bff.Add(teamMate.Id, new BestFriendForever { Player = teamMate });
                        }

                        if (match.WonTheMatch(id))
                        {
                            if (rbff.ContainsKey(teamMate.Id))
                            {
                                rbff[teamMate.Id].Occurrences++;
                            }
                            else
                            {
                                rbff.Add(teamMate.Id, new RealBestFriendForever { Player = teamMate });
                            }
                        }
                    }

                    if (match.IsOnRedTeam(id))
                    {
                        var d = "d";
                        if (preferredColor.ContainsKey(Red))
                        {
                            preferredColor[Red].Occurrences++;
                        }
                        else
                        {
                            preferredColor.Add(Red, new PreferredColor { Color = Red, Occurrences = 1 });
                        }

                        if (match.WonTheMatch(id))
                        {
                            if (winningColor.ContainsKey(Red))
                            {
                                winningColor[Red].Occurrences++;
                            }
                            else
                            {
                                winningColor.Add(Red, new WinningColor { Color = Red, Occurrences = 1 });
                            }
                        } 
                        else
                        {
                            var goalDiff = match.BlueScore - match.RedScore;
                            if (eae.ContainsKey(match.BluePlayer1.Id))
                            {
                                eae[match.BluePlayer1.Id].Occurrences++;
                                eae[match.BluePlayer1.Id].GoalDiff += goalDiff;
                            }
                            else
                            {
                                eae.Add(match.BluePlayer1.Id, new EvilArchEnemy { Player = match.BluePlayer1, GoalDiff = goalDiff });
                            }
                            
                            if (match.BluePlayer2.Id != null)
                            {
                                if (eae.ContainsKey(match.BluePlayer2.Id))
                                {
                                    eae[match.BluePlayer2.Id].Occurrences++;
                                    eae[match.BluePlayer2.Id].GoalDiff += goalDiff;
                                }
                                else
                                {
                                    eae.Add(match.BluePlayer2.Id, new EvilArchEnemy { Player = match.BluePlayer2, GoalDiff = goalDiff });
                                }
                            }
                        }

                    }
                    else
                    {
                        if (preferredColor.ContainsKey(Blue))
                        {
                            preferredColor[Blue].Occurrences++;
                        }
                        else
                        {
                            preferredColor.Add(Blue, new PreferredColor { Color = Blue, Occurrences = 1 });
                        }

                        if (match.WonTheMatch(id))
                        {
                            if (winningColor.ContainsKey(Blue))
                            {
                                winningColor[Blue].Occurrences++;
                            }
                            else
                            {
                                winningColor.Add(Blue, new WinningColor { Color = Blue, Occurrences = 1 });
                            }
                        } 
                        else
                        {
                            var goalDiff = match.BlueScore - match.RedScore;
                            if (eae.ContainsKey(match.RedPlayer1.Id))
                            {
                                eae[match.RedPlayer1.Id].Occurrences++;
                                eae[match.RedPlayer1.Id].GoalDiff = goalDiff;
                            }
                            else
                            {
                                eae.Add(match.RedPlayer1.Id, new EvilArchEnemy { Player = match.RedPlayer1, GoalDiff = goalDiff });
                            }

                            if (match.RedPlayer2.Id != null)
                            {
                                if (eae.ContainsKey(match.RedPlayer2.Id))
                                {
                                    eae[match.RedPlayer2.Id].Occurrences++;
                                    eae[match.RedPlayer2.Id].GoalDiff = goalDiff;
                                }
                                else
                                {
                                    eae.Add(match.RedPlayer2.Id, new EvilArchEnemy { Player = match.RedPlayer2, GoalDiff = goalDiff });
                                }
                            }
                        }
                    }
                    
                    stats.Played++;
                    stats.Won = match.WonTheMatch(id) ? ++stats.Won : stats.Won;
                    stats.Lost = !match.WonTheMatch(id) ? ++stats.Lost : stats.Lost;
                    stats.PlayedToday = match.GameOverTime.ToLocalTime().Day == DateTime.Now.Day ? ++stats.PlayedToday : stats.PlayedToday;
                    stats.PlayedLast7Days = match.GameOverTime.ToLocalTime() > DateTime.Now.AddDays(-7)? ++stats.PlayedLast7Days : stats.PlayedLast7Days;
                    stats.PlayedLast30Days = match.GameOverTime.ToLocalTime() > DateTime.Now.AddDays(-30) ? ++stats.PlayedLast30Days : stats.PlayedLast30Days;
                }

                stats.Bff = bff.OrderByDescending(i => i.Value.Occurrences).Select(i => i.Value).FirstOrDefault();
                stats.Rbff = rbff.OrderByDescending(i => i.Value.Occurrences).Select(i => i.Value).FirstOrDefault();
                stats.Eae = eae.OrderByDescending(i => i.Value.Occurrences).ThenByDescending(i => i.Value.GoalDiff).Select(i => i.Value).FirstOrDefault();
                stats.PreferredColor = preferredColor.OrderByDescending(i => i.Value.Occurrences).Select(i => i.Value).FirstOrDefault();
                stats.WinningColor = winningColor.OrderByDescending(i => i.Value.Occurrences).Select(i => i.Value).FirstOrDefault();
                stats.LatestMatch = playedMatches.Last();

                return View(stats);
            }

            return View();
        }
    }
}
