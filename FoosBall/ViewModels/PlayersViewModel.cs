namespace FoosBall.ViewModels
{
    using System.Collections.Generic;

    using FoosBall.Models;
    using FoosBall.Models.Base;

    public class PlayersViewModel : FoosBallViewModel
    {
        public IEnumerable<Player> AllPlayers { get; set; }
    }
}