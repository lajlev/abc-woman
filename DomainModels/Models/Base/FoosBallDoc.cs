namespace FoosBall.Main.Models.Base
{
    using FoosBall.Main.MongoDB.Bson;

    public abstract class FoosBallDoc
    {
        public BsonObjectId Id { get; set; }

        public BsonDateTime Created { get; set; }

        public BsonObjectId CreatedBy { get; set; }

        public BsonDateTime Updated { get; set; }

        public BsonObjectId UpdatedBy { get; set; }
    }
}