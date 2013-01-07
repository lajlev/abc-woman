namespace FoosBall.Models.Custom
{
    using FoosBall.Models.Domain;

    public class RatingDifference
    {
        public double Rating { get; set; }

        public Match Match { get; set; }
    }
}