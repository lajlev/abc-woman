using System.Web.Mvc;
using FoosBall.Main;
using FoosBall.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FoosBall.Controllers
{
    public class AccountController : BaseController
    {
        // GET: /Account/LogOn
        public ActionResult LogOn()
        {
            if (Session["IsLoggedIn"] == null || Session["IsLoggedIn"].ToString() == "false")
            {
                var authCookie = System.Web.HttpContext.Current.Request.Cookies.Get("FoosBallAuth");
                if (authCookie != null && authCookie["Token"] != null)
                {
                    var autoLoginCollection = this.Dbh.GetCollection<AutoLogin>("AutoLogin");
                    var autoLoginToken = autoLoginCollection.FindOne(Query.EQ("Token", authCookie["Token"]));

                    if (autoLoginToken != null) 
                    {
                        var playerCollection = this.Dbh.GetCollection<Player>("Players");
                        var player = playerCollection.FindOne(Query.EQ("Email", autoLoginToken.Email.ToLower()));
                    
                        if (Login(player))
                        {
                            // Go back to where we were before logging in
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }

                }

            }
            return View(new LogOnModel { RefUrl = Request.UrlReferrer.ToString() });
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var player = playerCollection.FindOne(Query.EQ("Email", model.Email.ToLower()));
            
            // If the email matches a player then check password
            if (player != null)
            {
                // If password is valid
                if (player.Password == Md5.CalculateMd5(model.Password))
                {
                    if (Login(player)) {
                        // Go back to where we were before logging in
                        return Redirect(model.RefUrl);
                    }
                }
            }

            model.LogOnError = true;
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            Session.Clear();
            RemoveRememberMeCookie();
            // Go back to where we were before logging in
            return Redirect(Request.UrlReferrer.ToString());
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(Player model)
        {
            var email = model.Email.ToLower();
            var name = model.Name;
            var password = Md5.CalculateMd5(model.Password);
            var department = model.Department;
            var position = model.Position;
            var nickname = model.NickName;

            var playerCollection = this.Dbh.GetCollection<Player>("Players");

            var newPlayer = new Player
                                {
                                    Email = email,
                                    Name = name,
                                    Password = password,
                                    Department = department,
                                    NickName = nickname,
                                    Position = position,
                                    Won = 0,
                                    Lost = 0,
                                    Played = 0
                                };

            playerCollection.Save(newPlayer);

            Login(newPlayer);
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/PlayerEmailExists
        [HttpPost]
        public JsonResult PlayerEmailExists(string email)
        {
            var query = Query.EQ("Email", email.ToLower());
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var player = playerCollection.FindOne(query);

            if (player != null)
            {
                return Json(new ExistsResponse { Exists = true, Name = player.Name, Email = player.Email });
            }
    
            return Json(new ExistsResponse { Exists = false, Name = null, Email = null });

        }

        //
        // POST: /Account/PlayerNameExists
        [HttpPost]
        public JsonResult PlayerNameExists(string name)
        {
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var query = Query.EQ("Name", name);
            var player = playerCollection.FindOne(query);

            if (player != null)
            {
                return Json(new ExistsResponse { Exists = true, Name = player.Name, Email = player.Email });
            }

            return Json(new ExistsResponse { Exists = false, Name = null, Email = null });
        }
    }
}
