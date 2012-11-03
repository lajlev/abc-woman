namespace FoosBall.Models.Views
{
    using System.Collections.Generic;

    public class PlayerDetailsViewModel
    {
        public Player Player { get; set; }

        public List<Match> PlayedMatches { get; set; }

    }
}