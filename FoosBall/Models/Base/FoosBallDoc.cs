namespace FoosBall.Models.Base
{
    using MongoDB.Bson;

    public abstract class FoosBallDoc
    {
        public BsonObjectId Id { get; set; }
    }
}