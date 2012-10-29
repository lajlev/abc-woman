// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatsController.cs" company="Trustpilot">
//   Trustpilot A/S 2012
// </copyright>
// <summary>
//   Defines the StatsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FoosBall.Controllers
{
    using System.Web.Mvc;

    using FoosBall.Models;

    using MongoDB.Driver.Builders;

    /// <summary>
    /// The stats controller.
    /// </summary>
    public class StatsController : BaseController
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
            var playerCollection = this.Dbh.GetCollection<Player>("Players");
            var noOfPlayers = playerCollection.Count();

            var model = new StatsModel()
                {
                    NoOfPlayers = noOfPlayers
                };


            return this.View(model);
        }
    }
}
