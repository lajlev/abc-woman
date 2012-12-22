namespace FoosBall.Controllers
{
    using System.Web.Mvc;

    using FoosBall.Models.ViewModels;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = HomeViewModel.GetMessage();

            return View();
        }
        
        public ActionResult Features()
        {
            return View();
        }
    }
}