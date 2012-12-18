namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Main;
    using FoosBall.Models;
    using FoosBall.Models.Views;
    using FoosBall.ViewModels;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class PlayersController : BaseController
    {
        public ActionResult Index()
        {
            var playerCollection = this.Dbh.GetCollection<Player>("Players").FindAll().SetSortOrder(SortBy.Descending("Rating")).ToList();

            return this.View(new PlayerViewModel { AllPlayers = playerCollection });
        }
    }
}