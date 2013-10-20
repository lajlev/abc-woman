namespace FoosBall.Controllers
{
    using System.Web.Mvc;
    using ControllerHelpers;
    using Main;
    using Models.Base;
    using Models.Domain;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult GetUser()
        {
            var currentUser = (Player)Session["User"];

            return Json(DbHelper.GetPlayer(currentUser.Id), JsonRequestBehavior.AllowGet);
        }

        public bool Login(Player player)
        {
            // Set or remove cookie for future auto-login
            if (player != null)
            {
                if (player.RememberMe)
                {
                    // Save an autologin token as cookie and in the Db
                    var playerCollection = Dbh.GetCollection<Player>("Players");
                    var autoLoginCollection = Dbh.GetCollection<AutoLogin>("AutoLogin");
                    var autoLogin = autoLoginCollection.FindOne(Query.EQ("Email", player.Email));

                    if (autoLogin == null)
                    {
                        autoLogin = new AutoLogin
                        {
                            Email = player.Email,
                            Token = GetAuthToken(player),
                        };
                        autoLoginCollection.Save(autoLogin);
                    }

                    CreateRememberMeCookie(player);
                    player.RememberMe = player.RememberMe;
                    playerCollection.Save(player);
                }
                else
                {
                    RemoveRememberMeCookie();
                }

                Session["Admin"] = Settings.AdminAccounts.Contains(player.Email);
                Session["IsLoggedIn"] = true;
                Session["User"] = player;

                return true;
            }

            return false;
        }
        
        [HttpGet]
        public void LogOn()
        {
            var autoLoginCollection = this.Dbh.GetCollection<AutoLogin>("AutoLogin");
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            
            if (Session["IsLoggedIn"] != null && Session["IsLoggedIn"].ToString() != "false")
            {
                return;
            }

            var authCookie = this.Request.Cookies.Get("FoosBallAuth");

            if (authCookie == null || authCookie["Token"] == null)
            {
                return;
            }

            var autoLoginToken = autoLoginCollection.FindOne(Query.EQ("Token", authCookie["Token"]));
            
            if (autoLoginToken == null)
            {
                return;
            }

            var player = playerCollection.FindOne(Query.EQ("Email", autoLoginToken.Email.ToLower()));

            Login(player);
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
                else
                {
                    loginInfo.Message = "Wrong user name or password";
                    loginInfo.Success = false;
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
            var validation = new Validation();

            var response = validation.ValidateNewUserData(userEmail, userName, userPassword);
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
        public ActionResult Edit(string email, string name, string oldPassword = "", string newPassword = "", bool Deactivated = false)
        {
            var response = new AjaxResponse { Success = false };
            var currentUser = (Player)Session["User"];
            var player = DbHelper.GetPlayer(currentUser.Id);
            var newMd5Password = Md5.CalculateMd5(newPassword);
            var validation = new Validation();

            if (player == null)
            {
                response.Message = "You have to be logged in to change user information";
                return Json(response);
            }

            if (!validation.ValidateEmail(email))
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
    }
}
