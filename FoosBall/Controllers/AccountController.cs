namespace FoosBall.Controllers
{
    using System.Web.Mvc;
    using FoosBall.Main;
    using FoosBall.Models;
    using FoosBall.Models.Views;

    using MongoDB.Driver.Builders;
    
    public class AccountController : BaseController
    {
        public ActionResult LogOn()
        {
            if (Session["IsLoggedIn"] == null || Session["IsLoggedIn"].ToString() == "false")
            {
                var authCookie = this.Request.Cookies.Get("FoosBallAuth");
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
                            var referrer = this.Request.UrlReferrer;
                            if (referrer != null)
                            {
                                return this.Redirect(referrer.ToString());
                            }
                        } 
                    }
                }
            }

            var urlReferrer = this.Request.UrlReferrer;
            if (urlReferrer != null)
            {
                return this.View(new LogOnViewModel { RefUrl = urlReferrer.ToString() });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model)
        {
            var email = model.Email.ToLower();
            if (this.Settings.RequireDomainValidation)
            {
                email += "@" + this.Settings.Domain;
            }

            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var player = playerCollection.FindOne(Query.EQ("Email", email));
            
            // If the email matches a player then check password
            if (player != null)
            {
                // If password is valid
                if (player.Password == Md5.CalculateMd5(model.Password))
                {
                    if (Login(player))
                    {
                        // Go back to where we were before logging in
                        return Redirect(model.RefUrl);
                    }
                }
            }

            model.LogOnError = true;
            return View(model);
        }

        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            Session.Clear();
            RemoveRememberMeCookie();
            
            // Go back to where we were before logging in
            var urlReferrer = this.Request.UrlReferrer;
            if (urlReferrer != null)
            {
                return this.Redirect(urlReferrer.ToString());
            } 
                
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public ActionResult Register()
        {
            ViewBag.Settings = this.Settings;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(Player model)
        {
            var email = model.Email.ToLower();
            if (this.Settings.RequireDomainValidation)
            {
                email += "@" + this.Settings.Domain;
            }
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

        // GET Account/GetGravatarUrl/{emailPrefix}
        [HttpGet]
        public JsonResult GetGravatarUrl(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var gravatarUrl = Md5.GetGravatarEmailHash(email);
                return Json(new { url = gravatarUrl }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { url = string.Empty }, JsonRequestBehavior.AllowGet);
        }
    }
}
