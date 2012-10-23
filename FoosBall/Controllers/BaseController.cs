using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FoosBall.Main;
using FoosBall.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FoosBall.Controllers
{
    public class BaseController : Controller
    {
        private readonly MongoDatabase _dbh;
        public BaseController()
        {
            _dbh = Db.GetDataBaseHandle();
        }

       public bool Login(Player player)
        {
            // Set or remove cookie for future auto-login
            if (player.RememberMe)
            {
                // Save an autologin token as cookie and in the Db
                var playerCollection = _dbh.GetCollection<Player>("Players");
                var autoLoginCollection = _dbh.GetCollection<AutoLogin>("AutoLogin");
                var autoLogin = autoLoginCollection.FindOne(Query.EQ("Email", player.Email));

                if (autoLogin == null)
                {
                    autoLogin = new AutoLogin
                    {
                        Email = player.Email,
                        Token = GetAuthToken(player),
                        Created = DateTime.Now
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

            Session["IsLoggedIn"] = true;
            Session["User"] = player;

            return true;
        }

        public static string CalculateMd5(string input, Encoding useEncoding)
        {
            var cryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider();

            var inputBytes = useEncoding.GetBytes(input);
            inputBytes = cryptoService.ComputeHash(inputBytes);
            return BitConverter.ToString(inputBytes).Replace("-", "");
        }

        // Calculates a MD5 hash from the given string. 
        // (By using the default encoding)
        public static string CalculateMd5(string Input)
        {
            // That's just a shortcut to the base method
            return CalculateMd5(Input, System.Text.Encoding.Default);
        }

        // COOKIE DOUGH
        public void CreateRememberMeCookie(Player player)
        {
            HttpContext.Response.Cookies.Add(new HttpCookie("FoosBallAuth"));
            var httpCookie = ControllerContext.HttpContext.Response.Cookies["FoosBallAuth"];
            if (httpCookie != null) httpCookie["Token"] = GetAuthToken(player);
            if (httpCookie != null) httpCookie.Expires = DateTime.Now.AddDays(30);
        }

        public void RemoveRememberMeCookie()
        {
            ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("FoosBallAuth"));
            var httpCookie = ControllerContext.HttpContext.Response.Cookies["FoosBallAuth"];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddDays(-1);
        }
        
        public static string GetAuthToken(Player player)
        {
            return Md5.CalculateMd5(player.Id + player.Email + "FoosBall4Ever");
        }
        
    }
}
