using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.LogicsSite.Currency
{
    public class WorkWithCurrency: IWorkWithCurrency
    {
        public async Task<IList<CurrencyUi>> GetAllCurrencyAsync()
        {
            var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            var ret = await currency.ReadAllAsync();

            return ret;
        }
    }
}
