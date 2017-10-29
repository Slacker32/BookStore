using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.Logics
{
    interface IBookShop
    {
        bool Buy(BookUi book);
        bool Reversal(BookUi book);
    }
}
