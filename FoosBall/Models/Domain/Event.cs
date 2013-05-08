namespace FoosBall.Models.Domain
{
    using FoosBall.Models.Base;

    public class Event : FoosBallDoc
    {
        public Event()
        {
            this.Index = System.DateTime.Now.Ticks;
        }

        public long Index { get; set; }

        public EventType EventType { get; set; }

        public Player Player { get; set; }

        public Match Match { get; set; }
    }
}