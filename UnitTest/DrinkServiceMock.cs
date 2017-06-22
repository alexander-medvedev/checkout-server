using Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    public class DrinkServiceMock : IDrinkService
    {
        public Dictionary<string, int> Drinks { get; set; }

        public Task Clear() => Task.Run(() => this.Drinks.Clear());

        public Task<bool> Delete(string name) => Task.Run(() => this.Drinks.Remove(name));

        public Task<Drink> Find(string name) =>
            Task.Run(() => this.Drinks.TryGetValue(name, out int count) ? new Drink { Name = name, Count = count } : null);

        public Task<bool> Insert(Drink drink)
        {
            if (this.Drinks.ContainsKey(drink.Name))
            {
                return Task.FromResult(false);
            }

            this.Drinks[drink.Name] = drink.Count;
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Drink>> List() =>
            Task.FromResult(this.Drinks.Select(x => new Drink { Name = x.Key, Count = x.Value }));

        public Task<bool> Update(Drink drink)
        {
            if (!this.Drinks.ContainsKey(drink.Name))
            {
                return Task.FromResult(false);
            }

            this.Drinks[drink.Name] = drink.Count;
            return Task.FromResult(true);
        }
    }
}
