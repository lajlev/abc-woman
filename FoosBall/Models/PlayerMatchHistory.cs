namespace FoosBall.Models
{
    using MongoDB.Bson;

    public class PlayerMatchHistory : FoosBallDoc
    {
        public BsonObjectId PlayerId { get; set; }

        public BsonObjectId MatchId { get; set; }

        public Player Player { get; set; }

        public Match Match { get; set; }
    }
}