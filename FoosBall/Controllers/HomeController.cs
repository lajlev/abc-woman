namespace FoosBall.Controllers
{
    using System.Web.Mvc;

    using FoosBall.Models.ViewModels;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var viewModel = new HomeViewModel();
            return View(viewModel);
        }
        
        public ActionResult Features()
        {
            return View(new FeaturesViewModel { Settings = this.Settings });
        }
    }
}