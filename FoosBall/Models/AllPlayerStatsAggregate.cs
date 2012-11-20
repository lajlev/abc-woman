namespace FoosBall.Models
{
    public class AllPlayerStatsAggregate
    {
        public string MostFighting { get; set; }

        public string MostWinning { get; set; }
        
        public string MostLosing { get; set; }
        
        public string TopRanked { get; set; }
        
        public string BottomRanked { get; set; }
        
        public string MostWinsInARow { get; set; }
        
        public string MostLossesInARow { get; set; }
        
        public string BiggestRatingWin { get; set; }
        
        public string PlayedToday { get; set; }
    }
}