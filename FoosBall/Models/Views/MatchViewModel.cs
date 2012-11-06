namespace FoosBall.Models.Views
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class MatchViewModel
    {
        public IEnumerable<Match> PlayedMatches { get; set; }

        public IEnumerable<Match> PendingMatches { get; set; }

        public IEnumerable<SelectListItem> SelectPlayers { get; set; }
    }
}