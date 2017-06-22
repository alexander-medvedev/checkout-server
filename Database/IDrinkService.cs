using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database
{
    public interface IDrinkService
    {
        Task<bool> Insert(Drink drink);

        Task<bool> Update(Drink drink);

        Task<bool> Delete(string name);

        Task<IEnumerable<Drink>> List();

        Task<Drink> Find(string name);

        Task Clear();
    }
}
