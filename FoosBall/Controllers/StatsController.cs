
namespace FoosBall.Controllers
{
    using System.Web.Mvc;

    using FoosBall.Models;

    using MongoDB.Driver.Builders;

    public class StatsController : BaseController
    {
        public ActionResult Index(string playerId)
        {
            var playersCollection = Dbh.GetCollection<Player>("Players");
            var matchCollection = Dbh.GetCollection<Match>("Matches");

            var player = playersCollection.FindOne(Query.EQ("_id", playerId));


            return View();
        }

    }
}
