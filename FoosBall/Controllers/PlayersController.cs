using System.Linq;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using FoosBall.Models;
using FoosBall.Main;

namespace FoosBall.Controllers
{
    public class PlayersController : Controller
    {
        private readonly MongoDatabase _dbh;
        public PlayersController()
        {
            _dbh = Db.GetDataBaseHandle();
        }

        //
        // GET: /Player/
        public ActionResult Index()
        {
            var playerCollection = _dbh.GetCollection<Player>("Players").FindAll().SetSortOrder(SortBy.Descending("Rating")).ToList();

            return View(new PlayerModel { Players = playerCollection });
        }

        //
        // GET: /Player/Delete/{id}
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var currentUser = (Player)Session["User"];
            var playerCollection = _dbh.GetCollection<Player>("Players");

            if (currentUser != null && currentUser.Id == id)
            {
                var query = Query.EQ("_id", ObjectId.Parse(id));
                if (playerCollection != null) playerCollection.Remove(query);
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Player/Edit/{id}
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var currentUser = (Player)Session["User"];
            var playerCollection = _dbh.GetCollection<Player>("Players");

            var query = Query.EQ("_id", ObjectId.Parse(id));
            var player = playerCollection.FindOne(query);

            if (currentUser != null && currentUser.Id == player.Id)
            {
                return View(player);
            }

            return RedirectToAction("Index");
        }

        //
        // POST: /Player/Edit/{id}
        [HttpPost]
        public ActionResult Edit(FormCollection formValues)
        {
            var playerId = formValues.GetValue("player-id").AttemptedValue;
            var playerCollection = _dbh.GetCollection<Player>("Players");

            var query = Query.EQ("_id", ObjectId.Parse(playerId));
            var player = playerCollection.FindOne(query);

            player.Name = formValues.GetValue("Name").AttemptedValue;
            player.Email = formValues.GetValue("Email").AttemptedValue;
            player.Department = formValues.GetValue("Department").AttemptedValue;
            
            playerCollection.Save(player);

            return RedirectToAction("Index");
        }
    }

}

