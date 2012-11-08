namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Models;
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

        //[HttpPost]
        //public ActionResult CopyMongoDb()
        //{
        //    var playerCollection = Dbh.GetCollection<Player>("Players");
        //    var matchCollection = Dbh.GetCollection<Match>("Match");
        //}
    }
}
