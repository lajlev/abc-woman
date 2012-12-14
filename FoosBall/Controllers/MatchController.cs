namespace FoosBall.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Main;
    using FoosBall.Models;
    using FoosBall.Models.Views;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class MatchController : BaseController
    {       
        // GET: /Match/
        public ActionResult Index()
        {
            // Fetch all players to display in a <select>
            var playerCollection = this.Dbh.GetCollection<Player>("Players").FindAll().SetSortOrder(SortBy.Ascending("Name")).ToList();
            
            // Fetch all FoosBall matches
            var playedMatches =
                this.Dbh.GetCollection<Match>("Matches")
                    .Find(Query.NE("GameOverTime", BsonDateTime.Create(DateTime.MinValue)))
                    .ToList().OrderByDescending(x => x.GameOverTime);

            var pendingMatches =
                this.Dbh.GetCollection<Match>("Matches")
                    .Find(Query.EQ("GameOverTime", BsonDateTime.Create(DateTime.MinValue)))
                    .ToList()
                    .OrderByDescending(x => x.CreationTime);

            // Create content for the <select> 
            var selectItems = playerCollection
                .Select(team => new SelectListItem { Selected = false, Text = team.Name, Value = team.Id.ToString() })
                .ToList();

            var played = playedMatches.OrderByDescending(x => x.GameOverTime);
            var pending = pendingMatches.OrderByDescending(x => x.CreationTime);

            return View(new MatchViewModel { PlayedMatches = played, PendingMatches = pending, SelectPlayers = selectItems });
        }
        
        // POST: /Match/Create/{FormCollection}
        [HttpPost]
        public ActionResult Create(FormCollection formValues)
        {
            var r1 = formValues.GetValue("red-player-1").AttemptedValue;
            var r2 = formValues.GetValue("red-player-2").AttemptedValue;
            var b1 = formValues.GetValue("blue-player-1").AttemptedValue;
            var b2 = formValues.GetValue("blue-player-2").AttemptedValue;
            var currentUser = (Player)Session["User"];

            if (currentUser != null)
            {
                // only try to create a match if properties are set correctly
                if (!string.IsNullOrEmpty(r1) && !string.IsNullOrEmpty(b1))
                {
                    var matchCollection = this.Dbh.GetCollection<Match>("Matches");
                    var playerCollection = this.Dbh.GetCollection<Player>("Players");
                    var redPlayer1 = string.IsNullOrEmpty(r1) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(r1)));
                    var redPlayer2 = string.IsNullOrEmpty(r2) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(r2)));
                    var bluePlayer1 = string.IsNullOrEmpty(b1) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(b1)));
                    var bluePlayer2 = string.IsNullOrEmpty(b2) ? new Player() : playerCollection.FindOne(Query.EQ("_id", BsonObjectId.Create(b2)));

                    var newMatch = new Match
                                        {
                                            RedPlayer1 = redPlayer1,
                                            RedPlayer2 = redPlayer2,
                                            BluePlayer1 = bluePlayer1,
                                            BluePlayer2 = bluePlayer2,
                                            CreationTime = new BsonDateTime(DateTime.Now),
                                            GameOverTime = new BsonDateTime(DateTime.MinValue),
                                            Created = new BsonDateTime(DateTime.Now),
                                            CreatedBy = currentUser.Id
                                        };

                    // Save to Db
                    matchCollection.Save(newMatch);
                    Events.SubmitEvent("Create", "Match", newMatch, currentUser.Id);
                }
            }   
         
            return RedirectToAction("Index");
        }
        
        // GET: /Match/SaveMatchResult/{id}
        [HttpGet]
        public ActionResult SaveMatchResult(string id)
        {
            var currentUser = (Player)Session["User"];
            var matchCollection = this.Dbh.GetCollection<Match>("Matches");
            var query = Query.EQ("_id", BsonObjectId.Create(id));
            var match = matchCollection.FindOne(query);

            if (currentUser != null && match.ContainsPlayer(currentUser.Id))
            {
                return View("SaveMatchResult", match);
            }

            return RedirectToAction("Index");
        }

        // POST: /Match/SaveMatchResult/{FormCollection}
        [HttpPost]
        public ActionResult SaveMatchResult(FormCollection formValues)
        {
            var redScore = formValues.GetValue("team-red-score").AttemptedValue;
            var blueScore = formValues.GetValue("team-blue-score").AttemptedValue;
            
            if (string.IsNullOrEmpty(redScore) == false && string.IsNullOrEmpty(blueScore) == false)
            {
                var matchCollection = this.Dbh.GetCollection<Match>("Matches");
                var playerCollection = this.Dbh.GetCollection<Player>("Players");

                var query = Query.EQ("_id", ObjectId.Parse(formValues.GetValue("match-id").AttemptedValue));
                var match = matchCollection.FindOne(query);

                // Update players from the match with players from the Db.
                if (match.RedPlayer1.Id != null)
                {
                    match.RedPlayer1 = playerCollection.FindOne(Query.EQ("_id", match.RedPlayer1.Id));
                }

                if (match.RedPlayer2.Id != null)
                {
                    match.RedPlayer2 = playerCollection.FindOne(Query.EQ("_id", match.RedPlayer2.Id));
                }
                
                if (match.BluePlayer1.Id != null)
                {
                    match.BluePlayer1 = playerCollection.FindOne(Query.EQ("_id", match.BluePlayer1.Id));
                }
                
                if (match.BluePlayer2.Id != null)
                {
                    match.BluePlayer2 = playerCollection.FindOne(Query.EQ("_id", match.BluePlayer2.Id));
                }

                var currentUser = (Player)Session["User"];
                if (currentUser != null && match.ContainsPlayer(currentUser.Id))
                {
                    // Get the scores
                    var intRedScore = int.Parse(redScore, NumberStyles.Float);
                    var intBlueScore = int.Parse(blueScore, NumberStyles.Float);
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
                    }
                    else
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
                        if (member.Id != null)
                        {
                            member.Rating += ratingModifier;
                            member.Won++;
                            member.Played++;
                            playerCollection.Save(member);
                        }
                    }

                    foreach (var member in losers.MatchTeam)
                    {
                        if (member.Id != null)
                        {
                            member.Rating -= ratingModifier;
                            member.Lost++;
                            member.Played++;
                            playerCollection.Save(member);
                        }
                    }

                    // Update match time stats
                    match.GameOverTime = new BsonDateTime(DateTime.Now);

                    // Save the data to Db
                    matchCollection.Save(match);
                    Events.SubmitEvent("Resolve", "Match", match, currentUser.Id);
                }
            }
            
            return RedirectToAction("Index");
        }
        
        // POST: /Match/Delete/{id}
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var currentUser = (Player)Session["User"];
            
            if (currentUser != null)
            {
                var matchCollection = this.Dbh.GetCollection<Match>("Matches");
                var query = Query.EQ("_id", BsonObjectId.Create(id));
                var match = matchCollection.FindOne(query);

                Events.SubmitEvent("Delete", "Match", match, currentUser.Id);
                matchCollection.Remove(query);
            }

            return RedirectToAction("Index");
        }
    }
}