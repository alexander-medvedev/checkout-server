using Database;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller to handle drink interactions
    /// </summary>
    public class DrinkController : ApiController
    {
        private readonly IDrinkService drinkService;

        /// <summary>
        /// Create a controller instance
        /// </summary>
        /// <param name="drinkService">Drinks storage service</param>
        public DrinkController(IDrinkService drinkService)
        {
            if (drinkService == null) throw new ArgumentNullException(nameof(drinkService));

            this.drinkService = drinkService;
        }

        /// <summary>
        /// Find a drink in the cart by name
        /// </summary>
        /// <param name="name">Drink name</param>
        /// <returns>OK result with drink model if the drink exists in the cart, NotFound overwise</returns>
        public async Task<IHttpActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var drink = await this.drinkService.Find(name);

            return drink == null
                ? (IHttpActionResult)this.NotFound()
                : this.Ok(new DrinkModel { Quantity = drink.Count });
        }

        /// <summary>
        /// Insert a drink to the cart (not required by task)
        /// </summary>
        /// <param name="name">Drink name</param>
        /// <param name="model">Drink model</param>
        /// <returns>OK result if drink does not exists in the cart, BadRequest otherwise</returns>
        public async Task<IHttpActionResult> Post(string name, DrinkModel model)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var drink = new Drink { Name = name, Count = model.Quantity };

            return await this.drinkService.Insert(drink)
                ? (IHttpActionResult)this.Ok()
                : this.BadRequest($"Drink {name} already exists in the cart.");
        }

        /// <summary>
        /// Update a drink in the cart (not required by task)
        /// </summary>
        /// <param name="name">Drink name</param>
        /// <param name="model">Drink model</param>
        /// <returns>OK resuls if drink exists in the cart, BadRequest otherwise</returns>
        public async Task<IHttpActionResult> Put(string name, DrinkModel model)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var drink = new Drink { Name = name, Count = model.Quantity };

            return await this.drinkService.Update(drink)
                ? (IHttpActionResult)this.Ok()
                : this.NotFound();
        }

        /// <summary>
        /// Delete a drink from the cart
        /// </summary>
        /// <param name="name">Drink name</param>
        /// <returns>OK result with drink model if the drink exists in the cart, NotFound overwise</returns>
        public async Task<IHttpActionResult> Delete(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return await this.drinkService.Delete(name)
                ? (IHttpActionResult)this.Ok()
                : this.NotFound();
        }
    }
}