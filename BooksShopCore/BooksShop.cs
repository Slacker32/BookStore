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
    public interface IBooksShop
    {
        IWorkWithBooks Books { get; set; }
        IWorkWithOrder Order { get; set; }
        IPreview Preview { get; set; }
        IPromoCode Promocode { get; set; }
        IWorkWithCurrency Currency { get; set; }

        //private BuyerUi TempBuyer { get; set; }
    }
    public sealed class BooksShop: IBooksShop
    {
        public IWorkWithBooks Books { get; set; }
        public IWorkWithOrder Order { get; set; }
        public IPreview Preview { get; set; }
        public IPromoCode Promocode { get; set; }
        public IWorkWithCurrency Currency { get; set; }

        private BuyerUi TempBuyer { get; set; }

        public BooksShop()
        {
            TempBuyer = new BuyerUi();

            this.Books = new WorkWithBooks();
            this.Order = new WorkWithOrder(TempBuyer);
            this.Preview = new Preview();
            this.Currency = new WorkWithCurrency();
        }

        public BooksShop(IBooksShop bookShop)
        {
            if (bookShop != null)
            {
                TempBuyer = new BuyerUi();

                this.Books = bookShop.Books;
                this.Order = bookShop.Order;
                this.Preview = bookShop.Preview;
                this.Currency = bookShop.Currency;
            }
        }

    }
}
