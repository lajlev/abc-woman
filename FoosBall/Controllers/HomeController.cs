namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using FoosBall.Models;
    using FoosBall.Models.Views;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = HomeViewModel.GetMessage();

            return View();
        }
        
        public ActionResult Version()
        {
            ViewBag.Message = HomeViewModel.GetVersion();
            return View();
        }
        
        [HttpGet]
        public string GetEventFeed()
        {
            var eventCollection = this.Dbh.GetCollection<Event>("Events");
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var events = eventCollection.FindAll();
            var players = playerCollection.FindAll().ToList();

            return players.ToJson();

            //return Json(players.Select(x => new { Name = x.Name, Id = x.Id.ToString() }), JsonRequestBehavior.AllowGet);
        }
    }
}