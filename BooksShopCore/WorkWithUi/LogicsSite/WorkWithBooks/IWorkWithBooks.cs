using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.LogicsSite.WorkWithBooks
{
    public interface IWorkWithBooks
    {
        IList<BookUi> ShowAllBooks(string languageCode, string currency);
        IList<BookUi> FindBooks(string searchStr, string languageCode, string currency);
        BookUi GetBook(int index, string languageCode, string currencyCode);

        Task<IList<BookUi>> ShowAllBooksAsync(string languageCode = null, string currencyCode = null);
        Task<IList<BookUi>> FindBooksAsync(string searchStr, string languageCode, string currencyCode);
        Task<BookUi> GetBookAsync(int index, string languageCode, string currencyCode);

    }

}
