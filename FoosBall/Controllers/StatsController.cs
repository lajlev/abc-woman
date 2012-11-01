namespace FoosBall.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using FoosBall.Models;
    using FoosBall.Main;
    using MongoDB.Driver.Builders;

    public class StatsController : BaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
