using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
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

            return View(model: new PlayerModel() { Players = playerCollection });
        }

        //
        // POST: /Player/Create
        [HttpPost]
        public ActionResult Create(FormCollection formValues)
        {
            // only try to insert new player if name is not null or empty
            if (String.IsNullOrEmpty(formValues.GetValue("Name").AttemptedValue) == false)
            {
                var playerCollection = _dbh.GetCollection<Player>("Players");

                var player = new Player
                                    {
                                        Name = formValues.GetValue("Name").AttemptedValue,
                                        Email = formValues.GetValue("Email").AttemptedValue,
                                        Department = formValues.GetValue("Department").AttemptedValue,
                                        ImageUrl = formValues.GetValue("ImageUrl").AttemptedValue,
                                        Won = 0,
                                        Lost = 0,
                                        Played = 0
                                    };

                playerCollection.Save(player);
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Player/Delete/{id}
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var playerCollection = _dbh.GetCollection<Player>("Players");

            var query = Query.EQ("_id", ObjectId.Parse(id));
            var safeModeResult = playerCollection.Remove(query);
            
            return RedirectToAction("Index");
        }

        //
        // GET: /Player/Edit/{id}
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var playerCollection = _dbh.GetCollection<Player>("Players");

            var query = Query.EQ("_id", ObjectId.Parse(id));
            var player = playerCollection.FindOne(query);

            return View(player);
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
            player.ImageUrl = formValues.GetValue("ImageUrl").AttemptedValue;
            
            playerCollection.Save(player);

            return RedirectToAction("Index");
        }
    }

}

