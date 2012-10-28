using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using FoosBall.Models;
using FoosBall.Main;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FoosBall.Controllers
{
    public class MatchController : Controller
    {       
        private readonly MongoDatabase _dbh;
        public MatchController()
        {
            _dbh = Db.GetDataBaseHandle();
        }

        //
        // GET: /Match/
        public ActionResult Index()
        {
            // Fetch all players to display in a <select>
            var playerCollection = _dbh.GetCollection<Player>("Players").FindAll().ToList();
            
            // Fetch all FoosBall matches. 
            var matchCollection = _dbh.GetCollection<Match>("Matches").FindAll().ToList();
            var playedMatches = new List<Match>();
            var pendingMatches = new List<Match>();
            
            // Divide mathes into resolved and unresolved matches
            foreach (var match in matchCollection)
            {
                if (match.GameOverTime != BsonDateTime.Create(DateTime.MinValue))
                {
                    playedMatches.Add(match);
                } 
                else
                {
                    pendingMatches.Add(match);
                }
            }

            // Create content for the <select> 
            var selectItems = playerCollection
                .Select(team => new SelectListItem { Selected = false, Text = team.Name, Value = team.Id.ToString() })
                .ToList();

            var played = playedMatches.OrderByDescending(a => a.GameOverTime);
            var pending = pendingMatches.OrderByDescending(a => a.CreationTime);

            return View(new MatchModel { PlayedMatches = played, PendingMatches = pending, SelectPlayers = selectItems });
            
        }
        
        //
        // POST: /Match/Create/{FormCollection}
        [HttpPost]
        public ActionResult Create(FormCollection formValues)
        {
            var r1 = formValues.GetValue("red-player-1").AttemptedValue;
            var r2 = formValues.GetValue("red-player-2").AttemptedValue;
            var b1 = formValues.GetValue("blue-player-1").AttemptedValue;
            var b2 = formValues.GetValue("blue-player-2").AttemptedValue;

            // only try to create a match if properties are set correctly
            if (!String.IsNullOrEmpty(r1) && !String.IsNullOrEmpty(b1))
            {
                var matchCollection = _dbh.GetCollection<Match>("Matches");
                var playerCollection = _dbh.GetCollection<Player>("Players");

                var redPlayer1 = (String.IsNullOrEmpty(r1)) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(r1)));
                var redPlayer2 = (String.IsNullOrEmpty(r2)) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(r2)));
                var bluePlayer1 = (String.IsNullOrEmpty(b1)) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(b1)));
                var bluePlayer2 = (String.IsNullOrEmpty(b2)) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(b2)));

                var newMatch = new Match
                                    {
                                        RedPlayer1 = redPlayer1,
                                        RedPlayer2 = redPlayer2,
                                        BluePlayer1 = bluePlayer1,
                                        BluePlayer2 = bluePlayer2,
                                        CreationTime = new BsonDateTime(DateTime.Now),
                                        GameOverTime = new BsonDateTime(DateTime.MinValue),
                                    };

                   matchCollection.Save(newMatch);
            }
            
            return RedirectToAction("Index");
        }
        
        // 
        // GET: /Match/SaveMatchResult/{id}
        [HttpGet]
        public ActionResult SaveMatchResult(string id)
        {

            var currentUser = (Player)Session["User"];
            var matchCollection = _dbh.GetCollection<Match>("Matches");
            var query = Query.EQ("_id", BsonObjectId.Create(id));
            var match = matchCollection.FindOne(query);

            if (currentUser != null && match.ContainsPlayer(currentUser.Id))
            {
                return View("SaveMatchResult", match);
            }

            return RedirectToAction("Index");
        }

        // 
        // POST: /Match/SaveMatchResult/{FormCollection}
        [HttpPost]
        public ActionResult SaveMatchResult(FormCollection formValues)
        {
            // Score = The teams score from the played FoosBall match
            // Rating = The players (Elo)rating based on won and lost games.
            // Modifier = The number with which a rating will go up or down based on match outcaome

            var redScore = formValues.GetValue("team-red-score").AttemptedValue;
            var blueScore = formValues.GetValue("team-blue-score").AttemptedValue;
            
            if (String.IsNullOrEmpty(redScore) == false && String.IsNullOrEmpty(blueScore) == false)
            {
                var matchCollection = _dbh.GetCollection<Match>("Matches");
                var playerCollection = _dbh.GetCollection<Player>("Players");
            
                var query = Query.EQ("_id", ObjectId.Parse(formValues.GetValue("match-id").AttemptedValue));
                var match = matchCollection.FindOne(query);

                var currentUser = (Player)Session["User"];
                if (currentUser != null && match.ContainsPlayer(currentUser.Id))
                {
                    // Get the scores
                    var intRedScore = Int32.Parse(redScore, NumberStyles.Float);
                    var intBlueScore = Int32.Parse(blueScore, NumberStyles.Float);
                    match.RedScore = intRedScore;
                    match.BlueScore = intBlueScore;

                    // Determine the winners and the losers
                    var winners = new Team();
                    var losers = new Team();
                    if (match.RedScore > match.BlueScore)
                    {
                        winners.MatchTeam.Add(match.RedPlayer1);
                        winners.MatchTeam.Add(match.RedPlayer2);
                        losers.MatchTeam.Add(match.BluePlayer1);
                        losers.MatchTeam.Add(match.BluePlayer2);
                    } else
                    {
                        winners.MatchTeam.Add(match.BluePlayer1);
                        winners.MatchTeam.Add(match.BluePlayer2);
                        losers.MatchTeam.Add(match.RedPlayer1);
                        losers.MatchTeam.Add(match.RedPlayer2);
                    }
                   
                    // Get the rating modifier
                    var ratingModifier = Rating.GetRatingModifier(winners.GetTeamRating(), losers.GetTeamRating());
                    
                    // Propagate the rating and stats to the team members of both teams
                    foreach (var member in winners.MatchTeam)
                    {
                        if (member.Id != null) {
                            member.Rating += ratingModifier;
                            member.Won++;
                            member.Played++;
                            playerCollection.Save(member);
                        }
                    }
                 
                    foreach (var member in losers.MatchTeam)
                    {
                        if (member.Id != null) {
                            member.Rating -= ratingModifier;
                            member.Lost++;
                            member.Played++;
                            playerCollection.Save(member);
                        }
                    }
                 
                    // Update match score/time stats
                    match.RedScore = intRedScore;
                    match.BlueScore = intBlueScore;
                    match.GameOverTime = new BsonDateTime(DateTime.Now);
                    
                    // Save the data to Db
                    matchCollection.Save(match);
                }
            }
            
            return RedirectToAction("Index");
        }
        
        //
        // POST: /Match/Delete/{id}
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var matchCollection = _dbh.GetCollection<Match>("Matches");

            var query = Query.EQ("_id", BsonObjectId.Create(id));
            matchCollection.Remove(query);
    
            return RedirectToAction("Index");
        }

    }
}
