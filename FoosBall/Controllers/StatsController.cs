// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatsController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the StatsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FoosBall.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    /// <summary>
    /// The stats controller.
    /// </summary>
    public class StatsController : Controller
    {
        /// <summary>
        /// GET: /Stats/
        /// Fetch all kinds of statistics on the FoosBall players and matches.
        /// Return the data in the index view
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            return this.View();
        }

    }
}
