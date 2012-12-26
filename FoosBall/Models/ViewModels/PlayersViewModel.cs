namespace FoosBall.Models.ViewModels
{
    using System.Collections.Generic;

    using FoosBall.Models.Domain;
    using FoosBall.Models.Base;

    public class PlayersViewModel : FoosBallViewModel
    {
        public IEnumerable<Player> AllPlayers { get; set; }
    }
}