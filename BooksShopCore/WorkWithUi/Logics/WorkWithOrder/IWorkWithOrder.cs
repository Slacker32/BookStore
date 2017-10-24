using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithOrder
{
    internal interface IWorkWithOrder
    {
        IList<BookUi> ShowListCurrentPurchases();
        bool ConfirmationOrder();
        bool CancelPurchase(int purchaseId);
        bool IsCompleted(int purchaseId);
        IList<BookUi> ApplyPromocode(string promocode);
    }

}
