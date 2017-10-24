using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithBooks
{
    internal interface IWorkWithBooks
    {
        IList<BookUi> ShowAllBooks();
        IList<BookUi> FindBooks(string searchStr);
        bool Buy(int bookId);
        bool Reversal(int bookId);
        IList<BookUi> ChangeCurrencyBook(int idCurrency);
    }

}
