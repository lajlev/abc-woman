namespace FoosBall.Models.Custom
{
    using FoosBall.Models.Domain;

    public class Streak
    {
        public Player Player { get; set; }

        public int Count { get; set; }
    }
}