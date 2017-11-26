using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithOrder
{
    public interface IWorkWithOrder
    {
        bool Buy(BookUi book, decimal amount, CurrencyUi currencyUi);
        bool Reversal(BookUi book);
        IList<PurchaseUi> ShowListCurrentPurchases();
        bool ConfirmationOrder();
        bool CancelPurchaseAfterConfirmation(int purchaseId);
        bool IsCompleted(int purchaseId);
        bool ApplyPromocode(string promocode);
    }

}
