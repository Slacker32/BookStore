using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithBooks
{
    public interface IWorkWithBooks
    {
        IList<BookUi> ShowAllBooks(string languageCode, string currency);
        IList<BookUi> FindBooks(string searchStr, string languageCode, string currency);

        IList<BookUi> ChangeCurrencyBook(int idCurrency, string languageCode);
    }

}
