namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Models.Domain;
    using MongoDB.Driver.Builders;

    public class PlayersController : BaseController
    {
        [HttpGet]
        public ActionResult GetActivePlayers()
        {
            var playerCollection = Dbh.GetCollection<Player>("Players")
                                           .Find(Query.EQ("Deactivated", false))
                                           .SetSortOrder(SortBy.Ascending("Name"))
                                           .ToList();

            return Json(playerCollection, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExperiencedPlayers()
        {
            var playerCollection = Dbh.GetCollection<Player>("Players")
                                            .FindAll()
                                            .SetSortOrder(SortBy.Ascending("Name"))
                                            .Where(x => x.Played > 0)
                                            .ToList();

            return Json(playerCollection, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRankedPlayers()
        {
            var playerCollection = Dbh.GetCollection<Player>("Players")
                                        .FindAll()
                                        .SetSortOrder(SortBy.Descending("Rating"))
                                        .Where(x => x.Played > 0 && x.Deactivated == false)
                                        .ToList();

            return Json(playerCollection, JsonRequestBehavior.AllowGet);
        }
    }
}