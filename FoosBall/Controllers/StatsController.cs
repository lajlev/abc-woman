namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using FoosBall.Models;
    using FoosBall.Main;
    using MongoDB.Driver.Builders;

    public class StatsController : BaseController
    {
        public ActionResult Index()
        {

            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var noOfPlayers = playerCollection.Count();
            var mostPlayed = DataAccessLayer.GetMostPlayed(playerCollection);
            // var mostWins = playerCollection.Find(Query.Exists("Won")).SetSortOrder(SortBy.Descending("Won")).First();
            // var mostLosses = playerCollection.Find(Query.Exists("Lost")).SetSortOrder(SortBy.Descending("Lost")).First();

            var model = new StatsModel()
                {
                    NoOfPlayers = noOfPlayers,
                    MostPlayed = mostPlayed
                };

            return this.View(model);
        }
    }
}
