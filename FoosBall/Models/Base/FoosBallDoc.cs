namespace FoosBall.Models.Base
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public abstract class FoosBallDoc
    {
        public BsonObjectId Id { get; set; }

        public BsonDateTime Created { get; set; }

        public BsonObjectId CreatedBy { get; set; }

        public BsonDateTime Updated { get; set; }

        public BsonObjectId UpdatedBy { get; set; }
    }
}