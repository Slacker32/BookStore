using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.LogicsSite.Currency
{
    public interface IWorkWithCurrency
    {
        Task<IList<CurrencyUi>> GetAllCurrencyAsync();
    }
}
