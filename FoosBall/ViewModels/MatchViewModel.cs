namespace FoosBall.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using FoosBall.Models;

    public class MatchViewModel
    {
        public IEnumerable<Match> PlayedMatches { get; set; }

        public IEnumerable<Match> PendingMatches { get; set; }

        public IEnumerable<SelectListItem> SelectPlayers { get; set; }
    }
}