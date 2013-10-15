namespace FoosBall.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using ControllerHelpers;
    using Main;
    using Models.Base;
    using Models.Domain;
    using Models.ViewModels;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult GetPlayer()
        {
            var currentUser = (Player)Session["User"];

            return Json(DbHelper.GetPlayer(currentUser.Id), JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public void LogOn()
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

                        Login(player);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult LogOn(string email, string password, string refUrl, bool rememberMe = false)
        {
            var loginEmail = email;
            var playerCollection = Dbh.GetCollection<Player>("Players");
            var player = playerCollection.FindOne(Query.EQ("Email", loginEmail));
            var loginInfo = new AjaxResponse();

            if (player != null)
            {
                if (player.Password == Md5.CalculateMd5(password))
                {
                    if (Login(player))
                    {
                        loginInfo.Message = "Success";
                        loginInfo.Data = GetSessionInfo();
                        loginInfo.Success = true;
                    }
                }
            }
            else
            {
                loginInfo.Message = "Wrong user name or password";
                loginInfo.Data = GetSessionInfo(refresh: true);
                loginInfo.Success = false;
            }

            return Json(loginInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            RemoveRememberMeCookie();
            
            // Go back to where we were before logging in
            var urlReferrer = Request.UrlReferrer;
            if (urlReferrer != null)
            {
                return Redirect(urlReferrer.ToString());
            } 
                
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register(string email, string name, string password)
        {
            var userEmail = email.ToLower();
            var userName = name;
            var userPassword = Md5.CalculateMd5(password);

            var response = ValidateNewUserData(userEmail, userName, userPassword);
            if (!response.Success)
            {
                return Json(response);
            }

            var playerCollection = Dbh.GetCollection<Player>("Players");
            var newPlayer = new Player
                                {
                                    Id = BsonObjectId.GenerateNewId().ToString(),
                                    Email = userEmail,
                                    Name = userName,
                                    Password = userPassword,
                                    Won = 0,
                                    Lost = 0,
                                    Played = 0
                                };

            playerCollection.Save(newPlayer);
            Login(newPlayer);
            Events.SubmitEvent(EventType.PlayerCreate, newPlayer, newPlayer.Id);

            response.Data = GetSession(refresh: true);

            return Json(response);
        }

        [HttpPost]
        public ActionResult Edit(string email, string name, string oldPassword = "", string newPassword = "")
        {
            var response = new AjaxResponse { Success = false };
            var currentUser = (Player)Session["User"];
            var player = DbHelper.GetPlayer(currentUser.Id);
            var newMd5Password = Md5.CalculateMd5(newPassword);

            if (player == null)
            {
                response.Message = "You have to be logged in to change user information";
                return Json(response);
            }

            if (!ValidateEmail(email))
            {
                response.Message = "You must provide a valid trustpilot email";
                return Json(response);
            }

            player.Email = string.IsNullOrEmpty(email) ? player.Email : email;
            player.Name = string.IsNullOrEmpty(name) ? player.Name : name;

            if (!string.IsNullOrEmpty(newPassword))
            {
                if (Md5.CalculateMd5(oldPassword) == player.Password)
                {
                    player.Password = newMd5Password;
                }
                else
                {
                    response.Message = "The old password is not correct";
                    return Json(response);
                }
            }

            DbHelper.SavePlayer(player);
            response.Success = true;
            response.Message = "User updated succesfully";
            response.Data = GetSession(refresh: true);

            return Json(response);
        }

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

        private AjaxResponse ValidateNewUserData(string email, string name, string password)
        {
            var response = new AjaxResponse { Success = false };

            if (!ValidateEmail(email))
            {
                response.Message = "You must provide a valid trustpilot email";
                return response;
            }

            if (PlayerEmailExists(email))
            {
                response.Message = "This email is already registered";
                return response;
            }

            if (string.IsNullOrEmpty(name))
            {
                response.Message = "You must provide a name";
                return response;
            }

            if (PlayerNameExists(name))
            {
                response.Message = "A player with this name is already registered";
                return response;
            }

            if (string.IsNullOrEmpty(password))
            {
                response.Message = "You must provide a password";
                return response;
            }

            response.Message = "User created succesfully";
            response.Success = true;
         
            return response;
        }

        private bool PlayerEmailExists(string email)
        {
            var query = Query.EQ("Email", email.ToLower());
            var playerCollection = Dbh.GetCollection<Player>("Players");
            var player = playerCollection.FindOne(query);

            if (player != null)
            {
                return true;
            }

            return false;
        }

        private bool PlayerNameExists(string name)
        {
            var playerCollection = Dbh.GetCollection<Player>("Players");
            var query = Query.EQ("Name", name);
            var player = playerCollection.FindOne(query);

            if (player != null)
            {
                return true;
            }

            return false;
        }

        private bool ValidateEmail(string email)
        {
            var domain = "@" + Settings.Domain;

            return !string.IsNullOrEmpty(email) &&
                   email.EndsWith(domain) &&
                   email.Count(x => x == '@') == 1 &&
                   email.Length > domain.Length;
        }
    }
}
