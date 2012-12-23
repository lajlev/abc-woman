namespace FoosBall.Controllers
{
    using System.Web.Mvc;
    using FoosBall.Main;
    using FoosBall.Models.Domain;
    using FoosBall.Models.ViewModels;

    using MongoDB.Bson;
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
            if (this.Settings.EnableDomainValidation)
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
            return View(new PlayerBaseDataViewModel { Player = new Player(), Settings = this.Settings });
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(PlayerBaseDataViewModel model)
        {
            var email = model.Player.Email.ToLower();
            if (this.Settings.EnableDomainValidation)
            {
                email += "@" + this.Settings.Domain;
            }

            var name = model.Player.Name;
            var password = Md5.CalculateMd5(model.Player.Password);
            var nickname = model.Player.NickName;

            var playerCollection = this.Dbh.GetCollection<Player>("Players");

            var newPlayer = new Player
                                {
                                    Id = BsonObjectId.GenerateNewId().ToString(),
                                    Email = email,
                                    Name = name,
                                    Password = password,
                                    NickName = nickname,
                                    Won = 0,
                                    Lost = 0,
                                    Played = 0
                                };

            playerCollection.Save(newPlayer);

            Login(newPlayer);

            Events.SubmitEvent("Create", "Player", newPlayer, newPlayer.Id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var currentUser = (Player)Session["User"];
            var playerCollection = this.Dbh.GetCollection<Player>("Players");

            var query = Query.EQ("_id", BsonObjectId.Parse(id));
            var player = playerCollection.FindOne(query);

            ViewBag.Settings = Settings;

            if (currentUser != null && (currentUser.Id == player.Id || currentUser.Email == this.Settings.AdminAccount))
            {
                return this.View(new PlayerBaseDataViewModel { Player = player, Settings = this.Settings });
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(PlayerBaseDataViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = (Player)Session["User"];

                if (this.Settings.EnableDomainValidation)
                {
                    viewModel.Player.Email += "@" + this.Settings.Domain;
                }

                if (currentUser != null
                    && (currentUser.Id == viewModel.Player.Id || currentUser.Email == this.Settings.AdminAccount))
                {
                    var playerCollection = this.Dbh.GetCollection<Player>("Players");
                    var query = Query.EQ("_id", BsonObjectId.Parse(viewModel.Player.Id));
                    var player = playerCollection.FindOne(query);

                    player.Email = string.IsNullOrEmpty(viewModel.Player.Email)
                                        ? player.Email
                                        : viewModel.Player.Email;
                    player.Name = string.IsNullOrEmpty(viewModel.Player.Name)
                                        ? player.Name
                                        : viewModel.Player.Name;
                    player.Password = string.IsNullOrEmpty(viewModel.Player.Password)
                                        ? player.Password
                                        : Md5.CalculateMd5(viewModel.Player.Password);
                    player.NickName = string.IsNullOrEmpty(viewModel.Player.NickName)
                                        ? player.NickName
                                        : viewModel.Player.NickName;

                    playerCollection.Save(player);
                }
            }

            // Go back to where we were before logging in
            var referrer = this.Request.UrlReferrer;
            if (referrer != null)
            {
                return this.Redirect(referrer.ToString());
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult PlayerEmailIsValid(string email)
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
