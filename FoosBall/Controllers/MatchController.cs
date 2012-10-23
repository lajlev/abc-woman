using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
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
                .Select(team => new SelectListItem() { Selected = false, Text = team.Name, Value = team.Id.ToString() })
                .ToList();
            return View(model: new MatchModel { PlayedMatches = playedMatches, PendingMatches = pendingMatches, SelectPlayers = selectItems });
            
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

                var newMatch = new Match()
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
            var matchCollection = _dbh.GetCollection<Match>("Matches");
            var query = Query.EQ("_id", BsonObjectId.Create(id));
            var match = matchCollection.FindOne(query);

            return View("SaveMatchResult", match);
        }

        // 
        // POST: /Match/SaveMatchResult/
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
                
                // Get the scores
                var intRedScore = System.Int32.Parse(redScore, NumberStyles.Float);
                var intBlueScore = System.Int32.Parse(blueScore, NumberStyles.Float);
                // Get average rating of the team
                var redAvgRating  = match.RedPlayer2.Name == null ? match.RedPlayer1.Rating : (match.RedPlayer1.Rating + match.RedPlayer2.Rating)/2;
                var blueAvgRating = match.BluePlayer2.Name == null ? match.BluePlayer1.Rating : (match.BluePlayer1.Rating + match.BluePlayer2.Rating)/2;
                // Determine the winners and the losers
                var winners = new List<Player>();
                var losers = new List<Player>();
                if (match.RedScore > match.BlueScore)
                {
                    winners.Add(match.RedPlayer1);
                    winners.Add(match.RedPlayer2);
                    losers.Add(match.BluePlayer1);
                    losers.Add(match.BluePlayer1);
                } else
                {
                    winners.Add(match.BluePlayer1);
                    winners.Add(match.BluePlayer1);
                    losers.Add(match.RedPlayer1);
                    losers.Add(match.RedPlayer2);
                }

//                // Get the rating modifier
//                var ratingModifier = Rating.GetRatingModifier(winner.GetTeamRating(), loser.GetTeamRating());
//                // Propagate the rating and stats to the team members of both teams
//                foreach (var member in winners)
//                {
//                    member.Rating += ratingModifier;
//                    member.Won++;
//                    member.Played++;
//                    playerCollection.Save(member);
//                }
//            
//                foreach (var member in losers)
//                {
//                    member.Rating -= ratingModifier;
//                    member.Lost++;
//                    member.Played++;
//                    playerCollection.Save(member);
//                }
//            
//                // Update match score/time stats
//                match.RedScore = System.Convert.ToInt32(formValues.GetValue("team-red-score").AttemptedValue, 10);
//                match.BlueScore = System.Convert.ToInt32(formValues.GetValue("team-blue-score").AttemptedValue, 10);
//                match.GameOverTime = new BsonDateTime(DateTime.Now);
//                
//                // Save the data to Db
//                matchCollection.Save(match);
            }
            
            return RedirectToAction("Index");
        }
        
        //
        // POST: /Match/Delete/5
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
