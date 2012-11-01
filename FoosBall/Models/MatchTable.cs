namespace FoosBall.Models
{
    using System;
    using System.Collections.Generic;

    public class MatchTable
    {
        public IEnumerable<Match> Matches { get; set; }

        public Player User { get; set; }
    }
}

