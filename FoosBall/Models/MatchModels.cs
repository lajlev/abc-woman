using System.Collections.Generic;
using System.Web.Mvc;
using MongoDB.Bson;

namespace FoosBall.Models
{
    public class Match : FoosBallDoc
    {
        public int RedScore { get; set; }
        public int BlueScore { get; set; }
        public Player RedPlayer1 { get; set; }
        public Player RedPlayer2 { get; set; }
        public Player BluePlayer1 { get; set; }
        public Player BluePlayer2 { get; set; }
        public BsonDateTime CreationTime { get; set; }
        public BsonDateTime GameOverTime { get; set; }
    }

    public class MatchModel
    {
        public IEnumerable<Match> PlayedMatches { get; set; }
        public IEnumerable<Match> PendingMatches { get; set; }
        public IEnumerable<SelectListItem> SelectPlayers { get; set; }
    }
}   
