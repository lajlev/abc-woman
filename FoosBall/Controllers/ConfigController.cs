namespace FoosBall.Controllers
{
    using System.Web.Mvc;

    public class ConfigController : BaseController
    {
        public ActionResult Show()
        {
            var configCollection = Dbh.GetCollection<Models.Config>("Config").FindOne();

            return View(configCollection);
        }

        [HttpPost]
        public ActionResult Save(FormCollection form)
        {
            var configCollection = Dbh.GetCollection<Models.Config>("Config");
            var config = configCollection.FindOne();

            config.Name = form.GetValue("Name").AttemptedValue;
            config.Version = form.GetValue("Version").AttemptedValue;
            config.Domain = form.GetValue("Domain").AttemptedValue;
            config.RequireDepartment = form.GetValue("RequireDepartment") != null;
            config.RequireDomainValidation = form.GetValue("RequireDomainValidation") != null;
            config.AllowOneOnOneMatches = form.GetValue("AllowOneOnOneMatches") != null;
            config.GenderSpecificMatches = form.GetValue("GenderSpecificMatches") != null;

            configCollection.Save(config);

            return this.RedirectToAction("Show", "Config");
        }
    }
}
