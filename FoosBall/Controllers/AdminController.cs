namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Main;
    using FoosBall.Models;
    using FoosBall.Models.Base;
    using FoosBall.Models.Views;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            var currentUser = (Player)Session["User"];

            if (currentUser != null && currentUser.Email == this.Settings.AdminAccount)
            {
                var playerCollection = Dbh.GetCollection<Player>("Players")
                        .FindAll()
                        .SetSortOrder(SortBy.Ascending("Name"))
                        .ToList()
                        .Select(team => new SelectListItem { Selected = false, Text = team.Name, Value = team.Id.AsString })
                        .ToList();

                return View(new ConfigViewModel { Settings = this.Settings, Users = playerCollection });
            }

            return this.Redirect("/Home/Index");
        }

        [HttpGet]
        public JsonResult GetConfig()
        {
            var currentUser = (Player)Session["User"];

            if (currentUser != null && currentUser.Email == this.Settings.AdminAccount)
            {
                var playerCollection = Dbh.GetCollection<Player>("Players")
                        .FindAll()
                        .SetSortOrder(SortBy.Ascending("Name"))
                        .ToList()
                        .Select(team => new SelectListItem { Selected = false, Text = team.Name, Value = team.Id.AsString })
                        .ToList()
                        .ToJson();

                return Json(new { Settings = this.Settings.ToJson(), Users = playerCollection }, JsonRequestBehavior.AllowGet);
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult Save(FormCollection form)
        {
            var configCollection = Dbh.GetCollection<Config>("Config");
            
            this.Settings.Name = form.GetValue("Name").AttemptedValue;
            this.Settings.Domain = form.GetValue("Domain").AttemptedValue;
            this.Settings.AdminAccount = form.GetValue("AdminAccount").AttemptedValue;
            this.Settings.RequireDomainValidation = form.GetValue("RequireDomainValidation") != null;
            this.Settings.AllowOneOnOneMatches = form.GetValue("AllowOneOnOneMatches") != null;
            this.Settings.GenderSpecificMatches = form.GetValue("GenderSpecificMatches") != null;

            configCollection.Save(this.Settings);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public JsonResult CopyProdData()
        {
            var dbhTo = new Db(Environment.Staging).Dbh;
            var dbhFrom = new Db().Dbh;

            var allMatches = dbhFrom.GetCollection<Match>("Matches").FindAll();
            var allPlayers = dbhFrom.GetCollection<Player>("Players").FindAll();

            var destinationMatches = dbhTo.GetCollection<Match>("Matches");
            var destinationPlayers = dbhTo.GetCollection<Player>("Players");

            destinationMatches.RemoveAll();
            destinationPlayers.RemoveAll();

            foreach (var match in allMatches)
            {
                destinationMatches.Save(match);
            }

            foreach (var player in allPlayers)
            {
                destinationPlayers.Save(player);
            }

            return Json(new { success = true });
        }
    }
}
