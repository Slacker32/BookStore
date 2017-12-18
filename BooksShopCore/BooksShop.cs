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

namespace BooksShopCore
{
    public class BooksShop
    {
        public IWorkWithBooks Books { get; set; }
        public IWorkWithOrder Order { get; set; }
        public IPreview Preview { get; set; }
        internal IPromoCode Promocode { get; set; }

        private BuyerUi TempBuyer { get; set; }
        
        public BooksShop()
        {
            TempBuyer = new BuyerUi();

            this.Books = new WorkWithBooks();
            this.Order = new WorkWithOrder(TempBuyer);
            this.Preview = new Preview();
        }

    }
}
