namespace FoosBall.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models.Domain;
    using Models.ViewModels;
    using MongoDB.Driver.Builders;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var viewModel = new HomeViewModel
                {
                    LatestEvents = GetLatestEvents()
                };
            
            return View(viewModel);
        }
        
        public ActionResult Features()
        {   
            return View(new FeaturesViewModel { Settings = Settings });
        }
        
        private List<Event> GetLatestEvents()
        {
            return Dbh.GetCollection<Event>("Events")
                      .FindAll()
                      .SetSortOrder(SortBy.Descending("Index"))
                      .Take(10)
                      .ToList();
        }
    }
}
