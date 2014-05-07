namespace FoosBall.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models.Base;
    using Models.Domain;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class LeaguesController : BaseController
    {
        [HttpGet]
        public ActionResult GetAllLeagues()
        {
            var leagueCollection = Dbh.GetCollection<League>("Leagues")
                                           .FindAll()
                                           .SetSortOrder(SortBy.Ascending("Name"))
                                           .ToList();

            return Json(leagueCollection, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllLeaguesByUserId(string userId)
        {
            var leagueCollection = Dbh.GetCollection<League>("Leagues")
                                           .Find(Query.EQ("Administrators", userId))
                                           .SetSortOrder(SortBy.Ascending("Name"))
                                           .ToList();

            return Json(leagueCollection, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLeague(ObjectId objectId)
        {
            var league = Dbh.GetCollection<League>("Leagues")
                                           .Find(Query.EQ("_id", objectId));

            return Json(league, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateLeague(string name)
        {
            var currentUser = (Player)Session["User"];

            if (currentUser == null)
            {
                return new HttpStatusCodeResult(400, "Access denied - You must be logged in to create a League");                
            }

            if (string.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(400, "Bad request - Missing a name property");
            }

            var existingLeague = Dbh.GetCollection<League>("Leagues")
                                           .Find(Query.EQ("Name", name));

            if (existingLeague.Any())
            {
                return Json(new AjaxResponse {Success = false, Message = "A league with that name already exists"});
            }

            var newLeague = new League
            {
                Name = name,
                Administrators = new List<string> { currentUser.Id },
                CreatedBy = currentUser.Id
            };

            Dbh.GetCollection<League>("Leagues").Save(newLeague);

            return Json(new AjaxResponse { Success = true, Message = "League has been created" });
        }
    }
}