namespace FoosBall.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Main;
    using FoosBall.Models;

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

            if (currentUser != null && currentUser.Id == player.Id)
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
            var name = formValues.GetValue("Name").AttemptedValue;
            var password = formValues.GetValue("Password").AttemptedValue;
            var department = formValues.GetValue("Department").AttemptedValue;
            var position = formValues.GetValue("Position").AttemptedValue;
            var nickname = formValues.GetValue("NickName").AttemptedValue;

            if (currentUser != null && currentUser.Id.ToString() == playerId)
            {
                var playerCollection = this.Dbh.GetCollection<Player>("Players");
                var query = Query.EQ("_id", ObjectId.Parse(playerId));
                var player = playerCollection.FindOne(query);

                player.Email = email.Length > 0 ? email : player.Email;
                player.Name = name.Length > 0 ? name : player.Name;
                player.Password = password.Length > 0 ? Md5.CalculateMd5(password) : player.Password;
                player.Department = department.Length > 0 ? department : player.Department;
                player.Position = position.Length > 0 ? position : player.Position;
                player.NickName = nickname.Length > 0 ? nickname : player.NickName;

                player.Name = formValues.GetValue("Name").AttemptedValue;
                player.Email = formValues.GetValue("Email").AttemptedValue;
                player.Department = formValues.GetValue("Department").AttemptedValue;
                playerCollection.Save(player);
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(string playerId)
        {
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var query = Query.EQ("_id", ObjectId.Parse(playerId));
            var player = playerCollection.FindOne(query);

            var matchCollection = this.Dbh.GetCollection<Match>("Matches").FindAll().SetSortOrder(SortBy.Descending("GameOverTime")).ToList();
            var playedMatches = new List<Match>();

            foreach (var match in matchCollection)
            {
                if (match.ContainsPlayer(player.Id))
                {
                    playedMatches.Add(match);
                }
            }

            return this.View(new PlayerDetails() { Player = player, PlayedMatches = playedMatches });
        }
    }
}