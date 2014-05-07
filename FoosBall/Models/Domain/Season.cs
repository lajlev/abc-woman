namespace FoosBall.Models.Domain
{
    using System;
    using Base;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Season : FoosBallDoc
    {
        public string Name { get; set; }

        public DateTime Closed { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ClosedBy { get; set; }
    }
}