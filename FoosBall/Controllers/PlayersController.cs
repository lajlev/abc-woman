namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Main;
    using FoosBall.Models;
    using FoosBall.Models.Views;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class PlayersController : BaseController
    {
        public ActionResult Index()
        {
            var playerCollection = this.Dbh.GetCollection<Player>("Players").FindAll().SetSortOrder(SortBy.Descending("Rating")).ToList();

            return this.View(new PlayerViewModel { Players = playerCollection });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var currentUser = (Player)Session["User"];
            var playerCollection = this.Dbh.GetCollection<Player>("Players");

            var query = Query.EQ("_id", ObjectId.Parse(id));
            var player = playerCollection.FindOne(query);

            ViewBag.Settings = Settings;

            if (currentUser != null && (currentUser.Id == player.Id || currentUser.Email == this.Settings.AdminAccount))
            {
                return this.View(player);
            }

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(FormCollection formValues)
        {
            var currentUser = (Player)Session["User"];
            var playerId = formValues.GetValue("player-id").AttemptedValue;
            var email = formValues.GetValue("Email").AttemptedValue.ToLower();

            if (this.Settings.EnableDomainValidation)
            {
                email += "@" + this.Settings.Domain;
            }
            
            var name = formValues.GetValue("Name").AttemptedValue;
            var password = formValues.GetValue("Password").AttemptedValue;
            var nickname = formValues.GetValue("NickName").AttemptedValue;
             
            if (currentUser != null && (currentUser.Id.ToString() == playerId || currentUser.Email == this.Settings.AdminAccount))
            {
                var playerCollection = this.Dbh.GetCollection<Player>("Players");
                var query = Query.EQ("_id", ObjectId.Parse(playerId));
                var player = playerCollection.FindOne(query);
                 
                player.Email = email.Length > 0 ? email : player.Email;
                player.Name = name.Length > 0 ? name : player.Name;
                player.Password = password.Length > 0 ? Md5.CalculateMd5(password) : player.Password;
                player.NickName = nickname.Length > 0 ? nickname : player.NickName;

                playerCollection.Save(player);
            }

            return this.RedirectToAction("Index");
        }
    }
}