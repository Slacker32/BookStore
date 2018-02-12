using BooksShopCore.WorkWithUi.EntityUi;
using BooksShopCore.WorkWithUi.LogicsSite;
using BooksShopCore.WorkWithUi.LogicsSite.Preview;
using BooksShopCore.WorkWithUi.LogicsSite.PromoCode;
using BooksShopCore.WorkWithUi.LogicsSite.WorkWithBooks;
using BooksShopCore.WorkWithUi.LogicsSite.WorkWithOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithUi.WorkWithDataStorage;
using BooksShopCore.WorkWithUi.LogicsSite.Currency;

namespace BooksShopCore
{
    public abstract class AbstractBooksShop
    {
        public virtual IWorkWithBooks Books { get; set; }
        public virtual IWorkWithOrder Order { get; set; }
        public virtual IPreview Preview { get; set; }
        internal virtual IPromoCode Promocode { get; set; }
        public virtual IWorkWithCurrency Currency { get; set; }

        //private BuyerUi TempBuyer { get; set; }
    }
    public sealed class BooksShop: AbstractBooksShop
    {
        //public IWorkWithBooks Books { get; set; }
        //public IWorkWithOrder Order { get; set; }
        //public IPreview Preview { get; set; }
        //internal IPromoCode Promocode { get; set; }
        //public IWorkWithCurrency Currency { get; set; }

        private BuyerUi TempBuyer { get; set; }

        public BooksShop()
        {
            TempBuyer = new BuyerUi();

            this.Books = new WorkWithBooks();
            this.Order = new WorkWithOrder(TempBuyer);
            this.Preview = new Preview();
            this.Currency = new WorkWithCurrency();
        }


    }
}
