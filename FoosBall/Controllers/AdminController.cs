namespace FoosBall.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Main;
    using FoosBall.Models;
    using FoosBall.Models.Base;
    using FoosBall.Models.Views;

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
                        .Select(team => new SelectListItem { Selected = false, Text = team.Name, Value = team.Id.ToString() })
                        .ToList();

                return View(new ConfigViewModel { Settings = this.Settings, Users = playerCollection });
            }

            return this.Redirect("/Home/Index");
        }

        [HttpPost]
        public ActionResult Save(FormCollection form)
        {
            var configCollection = Dbh.GetCollection<Models.Config>("Config");
            var playerCollection = Dbh.GetCollection<Player>("Players")
                    .FindAll()
                    .SetSortOrder(SortBy.Ascending("Name"))
                    .ToList()
                    .Select(team => new SelectListItem { Selected = false, Text = team.Name, Value = team.Id.ToString() })
                    .ToList();

            this.Settings.Name = form.GetValue("Name").AttemptedValue;
            this.Settings.Version = form.GetValue("Version").AttemptedValue;
            this.Settings.Domain = form.GetValue("Domain").AttemptedValue;
            this.Settings.AdminAccount = form.GetValue("AdminAccount").AttemptedValue;
            this.Settings.RequireDepartment = form.GetValue("RequireDepartment") != null;
            this.Settings.RequireDomainValidation = form.GetValue("RequireDomainValidation") != null;
            this.Settings.AllowOneOnOneMatches = form.GetValue("AllowOneOnOneMatches") != null;
            this.Settings.GenderSpecificMatches = form.GetValue("GenderSpecificMatches") != null;

            configCollection.Save(this.Settings);

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult CopyProdData(string environment = "Staging")
        {
            var dbhTo = environment == "Local" ? new Db(Environment.Local).Dbh : new Db(Environment.Staging).Dbh;
            var dbhFrom = new Db(Environment.Production).Dbh;

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

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult SyncPlayersStatistics()
        {

            var playersCollection = Dbh.GetCollection<Match>("Matches");
            var allMatches = Dbh.GetCollection<Match>("Matches").FindAll();

            foreach (var match in allMatches)
            {
                var players = new List<Player>
                    {
                        match.RedPlayer1, 
                        match.BluePlayer1
                    };

            }

            return RedirectToAction("Index", "Admin");
        }
    }
}
