namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Models;
    using FoosBall.ViewModels;

    using MongoDB.Driver.Builders;

    public class PlayersController : BaseController
    {
        public ActionResult Index()
        {
            var playerCollection = Dbh.GetCollection<Player>("Players")
                                        .FindAll()
                                        .SetSortOrder(SortBy.Descending("Rating"))
                                        .ToList();

            return this.View(new PlayersViewModel { AllPlayers = playerCollection });
        }
    }
}