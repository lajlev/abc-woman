using System.Collections.Generic;

namespace FoosBall.Models
{
    public class PlayerDetails
    {
        public Player Player { get; set; }

        public List<Match> PlayedMatches { get; set; }

    }
}