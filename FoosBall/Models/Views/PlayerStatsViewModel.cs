namespace FoosBall.Models.Views
{
    using System.Collections.Generic;

    public class PlayerStatsViewModel
    {
        public Player Player { get; set; }

        public int Played { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public BestFriendForever Bff{ get; set; }

        public RealBestFriendForever Rbff{ get; set; }

        public EvilArchEnemy Eae { get; set; }

        public PreferredColor PreferredColor { get; set; }

        public WinningColor WinningColor { get; set; }

        public int PlayedToday { get; set; }

        public int PlayedLast7Days { get; set; }

        public int PlayedLast30Days { get; set; }

        public Match LatestMatch { get; set; }

        public IEnumerable<Match> PlayedMatches { get; set; }
    }
}