namespace FoosBall.Models
{
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
            return (RedPlayer2.Id == null) ? RedPlayer1.Rating : (RedPlayer1.Rating + RedPlayer2.Rating);
        }

        public double GetBlueTeamRating()
        {
            return (BluePlayer2.Id == null) ? BluePlayer1.Rating : (BluePlayer1.Rating + BluePlayer2.Rating);
        }
    }
}   
