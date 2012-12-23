namespace FoosBall.Models
{
    using System.Collections.Generic;

    using FoosBall.Models.Domain;

    public class MatchTable
    {
        public IEnumerable<Match> Matches { get; set; }

        public Player User { get; set; }
    }
}