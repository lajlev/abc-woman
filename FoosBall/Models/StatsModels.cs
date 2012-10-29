namespace FoosBall.Models
{
    using System;

    /// <summary>
    /// The stats model.
    /// </summary>
    public class StatsModel
    {
        // Players
        public long NoOfPlayers { get; set; }

        public string MostPlayed { get; set; }

        public string MostWins { get; set; }

        public string MostLosses { get; set; }

        public string MostGoals { get; set; }

        // Matches
        public DateTime LatestMatch { get; set; }

        public long TotalPlayed { get; set; }

        public long TotalGoals { get; set; } 
    }
}