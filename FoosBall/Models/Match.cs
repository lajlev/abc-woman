namespace FoosBall.Models
{
    using FoosBall.Models.Base;

    using MongoDB.Bson;
    
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

        public bool WonTheMatch(BsonObjectId id)
        {
            return (IsOnRedTeam(id) && RedScore > BlueScore) || (IsOnBlueTeam(id) && BlueScore > RedScore);
        }

        public bool ContainsPlayer(BsonObjectId id)
        {
            return id == RedPlayer1.Id || id == RedPlayer2.Id || id == BluePlayer1.Id || id == BluePlayer2.Id;
        }

        public bool IsOnRedTeam(BsonObjectId id)
        {
            return id == RedPlayer1.Id || id == RedPlayer2.Id;
        }

        public bool IsOnBlueTeam(BsonObjectId id)
        {
            return id == BluePlayer1.Id || id == BluePlayer2.Id;
        }

        public Player GetPartner(BsonObjectId id)
        {
            if (id == RedPlayer1.Id)
            {
                return RedPlayer2;
            }

            if (id == RedPlayer2.Id)
            {
                return RedPlayer1;
            }
            
            if (id == BluePlayer1.Id)
            {
                return BluePlayer2;
            }
            
            if (id == BluePlayer2.Id)
            {
                return BluePlayer1;
            }
            
            return null;
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
            return (RedPlayer2.Id == null) ? RedPlayer1.Rating : (RedPlayer1.Rating + RedPlayer2.Rating);
        }

        public double GetBlueTeamRating()
        {
            return (BluePlayer2.Id == null) ? BluePlayer1.Rating : (BluePlayer1.Rating + BluePlayer2.Rating);
        }
    }
}   
