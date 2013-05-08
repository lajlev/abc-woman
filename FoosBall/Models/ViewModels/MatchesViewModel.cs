namespace FoosBall.Models.ViewModels
{
    using System.Collections.Generic;

    using FoosBall.Models.Base;
    using FoosBall.Models.Custom;
    using FoosBall.Models.Domain;

    public class MatchesViewModel : BaseViewModel
    {
        public IEnumerable<Match> PlayedMatches { get; set; }

        public IEnumerable<CustomSelectListItem> SelectPlayers { get; set; }
    }
}