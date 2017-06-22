using Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller to handle cart interactions
    /// </summary>
    public class CartController : ApiController
    {
        private readonly IDrinkService drinkService;

        /// <summary>
        /// Create a controller instance
        /// </summary>
        /// <param name="drinkService">Drinks storage service</param>
        public CartController(IDrinkService drinkService)
        {
            if (drinkService == null) throw new ArgumentNullException(nameof(drinkService));

            this.drinkService = drinkService;
        }

        /// <summary>
        /// List all drinks in the cart
        /// </summary>
        /// <returns>OK result with array of cart models</returns>
        public async Task<IHttpActionResult> Get()
        {
            var drinks = await this.drinkService.List();
            var results = drinks.Select(DrinkToModel).OrderBy(x => x.Drink).ToArray();
            return this.Ok(results);
        }

        /// <summary>
        /// Insert a drink to the cart
        /// </summary>
        /// <param name="model">Cart model</param>
        /// <returns>OK result if drink does not exists in the cart, BadRequest otherwise</returns>
        public async Task<IHttpActionResult> Post(CartModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return await this.drinkService.Insert(ModelToDrink(model))
                ? (IHttpActionResult)this.Ok()
                : this.BadRequest($"Drink {model.Drink} already exists in the cart.");
        }

        /// <summary>
        /// Update a drink in the cart
        /// </summary>
        /// <param name="model">Cart model</param>
        /// <returns>OK resuls if drink exists in the cart, BadRequest otherwise</returns>
        public async Task<IHttpActionResult> Put(CartModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return await this.drinkService.Update(ModelToDrink(model))
                ? (IHttpActionResult)this.Ok()
                : this.BadRequest($"Drink {model.Drink} does not exist in the cart.");
        }

        /// <summary>
        /// Clear the cart (not required by task)
        /// </summary>
        /// <returns>OK result</returns>
        public async Task<IHttpActionResult> Delete()
        {
            await this.drinkService.Clear();
            return this.Ok();
        }

        private static CartModel DrinkToModel(Drink drink) =>
            new CartModel { Drink = drink.Name, Quantity = drink.Count };

        private static Drink ModelToDrink(CartModel model) =>
           new Drink { Name = model.Drink, Count = model.Quantity };
    }
}
