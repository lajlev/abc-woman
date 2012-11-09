namespace FoosBall.Controllers
{
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
        public ActionResult CopyProdToStaging()
        {
            var dbhStaging = new Db(Environment.Staging).Dbh;

            var allMatches = Dbh.GetCollection<Match>("Matches").FindAll();
            var allPlayers = Dbh.GetCollection<Player>("Players").FindAll();

            var stagingMatches = dbhStaging.GetCollection<Match>("Matches");
            var stagingPlayers = dbhStaging.GetCollection<Player>("Players");

            stagingMatches.RemoveAll();
            stagingPlayers.RemoveAll();
            
            foreach (var match in allMatches)
            {
                stagingMatches.Save(match);
            }

            foreach (var player in allPlayers)
            {
                stagingPlayers.Save(player);                
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}
