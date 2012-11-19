namespace FoosBall.Controllers
{
    using System.Web.Mvc;

    using FoosBall.Models.Views;

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
    }
}