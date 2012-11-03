using System.Web.Mvc;
using FoosBall.Models;

namespace FoosBall.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = HomeModel.GetMessage();

            return View();
        }
        
        public ActionResult Version()
        {
            ViewBag.Message = HomeModel.GetVersion();
            return View();
        }
        
    }
}
