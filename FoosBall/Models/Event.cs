namespace FoosBall.Models
{
    using FoosBall.Models.Base;

    using MongoDB.Bson;

    public class Event : FoosBallDoc
    {
        public Event()
        {
            this.Index = System.DateTime.Now.Ticks;
        }

        public long Index { get; set; }

        public string Action { get; set; }

        public string Object { get; set; }

        public BsonObjectId SubjectId { get; set; }
    }
}