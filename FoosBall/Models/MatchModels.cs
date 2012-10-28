using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public bool ContainsPlayer(BsonObjectId id)
        {
            return id == RedPlayer1.Id || id == RedPlayer2.Id || id == BluePlayer1.Id || id == BluePlayer2.Id;
        }
        public int CountRedPlayers()
        {
            return (RedPlayer2.Id == null) ? 1 : 2;
        }
        public int CountBluePlayers()
        {
            return (BluePlayer2.Id == null) ? 1 : 2;
        }
        public double GetRedTeamRating()
        {
            return (RedPlayer2.Id == null) ? RedPlayer1.Rating : (RedPlayer1.Rating+RedPlayer2.Rating);
        }
        public double GetBlueTeamRating()
        {
            return (BluePlayer2.Id == null) ? BluePlayer1.Rating : (BluePlayer1.Rating + BluePlayer2.Rating);
        }
    }

    public class MatchTable
    {
        public IEnumerable<Match> Matches { get; set; }
        public Player User { get; set; }
    }

    public class MatchModel
    {
        public IEnumerable<Match> PlayedMatches { get; set; }
        public IEnumerable<Match> PendingMatches { get; set; }
        public IEnumerable<SelectListItem> SelectPlayers { get; set; }
    }

    public class Team
    {
        public Team()
        {
            MatchTeam = new List<Player>();
        }

        public List<Player> MatchTeam { get; set; }
        
        public double GetTeamRating()
        {
            return MatchTeam.Sum(player => player.Rating);
        }
    }
}   
