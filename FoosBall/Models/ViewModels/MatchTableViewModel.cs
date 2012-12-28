namespace FoosBall.Models.ViewModels
{
    using System.Collections.Generic;

    using FoosBall.Models.Domain;

    public class MatchTableViewModel
    {
        public IEnumerable<Match> Matches { get; set; }

        public Player User { get; set; }
    }
}