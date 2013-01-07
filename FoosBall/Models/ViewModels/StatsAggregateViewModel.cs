namespace FoosBall.Models.ViewModels
{
    using FoosBall.Models.Custom;
    using FoosBall.Models.Domain;

    public class StatsAggregateViewModel
    {
        public Player MostFights { get; set; }

        public Player MostWins { get; set; }

        public Player MostLosses { get; set; }

        public Player TopRanked { get; set; }

        public Player BottomRanked { get; set; }

        public Player HighestRatingEver { get; set; }

        public Player LowestRatingEver { get; set; }

        public Streak LongestWinningStreak { get; set; }

        public Streak LongestLosingStreak { get; set; }

        public RatingDifference BiggestRatingWin { get; set; }
    }
}
