using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// Simple mock drink storage service based on in-memory hashtable
    /// In reality this should use DbContext to connect to a DB with EntityFramework
    /// I use async everywhere to model this, even though the class does not need it
    /// </summary>
    public class DrinkService : IDrinkService
    {
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        private readonly Dictionary<string, Drink> drinks = new Dictionary<string, Drink>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Clear all drinks from the cart
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task Clear()
        {
            this.locker.EnterWriteLock();

            try
            {
                this.drinks.Clear();
            }
            finally
            {
                this.locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Delete a drink from the cart
        /// </summary>
        /// <param name="name">Drink name</param>
        /// <returns>Whether the drink was deleted</returns>
        public async Task<bool> Delete(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            this.locker.EnterWriteLock();

            try
            {
                return this.drinks.Remove(name);
            }
            finally
            {
                this.locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Find a drink in the cart
        /// </summary>
        /// <param name="name">Drink name</param>
        /// <returns>Drink entity if it exists, null overwise</returns>
        public async Task<Drink> Find(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            this.locker.EnterReadLock();

            try
            {
                return this.drinks.TryGetValue(name, out Drink drink) ? drink : null;
            }
            finally
            {
                this.locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Insert a drink to the cart
        /// </summary>
        /// <param name="drink">Drink entity</param>
        /// <returns>Whether the drink was inserted</returns>
        public async Task<bool> Insert(Drink drink)
        {
            if (drink == null) throw new ArgumentNullException(nameof(drink));
            if (string.IsNullOrEmpty(drink.Name)) throw new ArgumentNullException(nameof(drink.Name));

            this.locker.EnterWriteLock();

            try
            {
                if (this.drinks.ContainsKey(drink.Name))
                {
                    return false;
                }

                this.drinks[drink.Name] = drink;
                return true;
            }
            finally
            {
                this.locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// List all drinks in the cart
        /// </summary>
        /// <returns>Collection of drink entities</returns>
        public async Task<IEnumerable<Drink>> List()
        {
            this.locker.EnterReadLock();

            try
            {
                return this.drinks.Values.AsEnumerable();
            }
            finally
            {
                this.locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Update a drink in the cart
        /// </summary>
        /// <param name="drink">Drink entity</param>
        /// <returns>Whether the drink was updated</returns>
        public async Task<bool> Update(Drink drink)
        {
            if (drink == null) throw new ArgumentNullException(nameof(drink));
            if (string.IsNullOrEmpty(drink.Name)) throw new ArgumentNullException(nameof(drink.Name));

            this.locker.EnterWriteLock();

            try
            {
                if (!this.drinks.ContainsKey(drink.Name))
                {
                    return false;
                }

                this.drinks[drink.Name] = drink;
                return true;
            }
            finally
            {
                this.locker.ExitWriteLock();
            }
        }
    }
}
