using BooksShopCore.WorkWithUi.EntityUi;
using BooksShopCore.WorkWithUi.Logics;
using BooksShopCore.WorkWithUi.Logics.Preview;
using BooksShopCore.WorkWithUi.Logics.PromoCode;
using BooksShopCore.WorkWithUi.Logics.WorkWithBooks;
using BooksShopCore.WorkWithUi.Logics.WorkWithOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage;

namespace BooksShopCore
{
    public class BooksShop
    {
        public IWorkWithBooks Books { get; set; }
        public IWorkWithOrder Order { get; set; }
        internal IPreview Preview { get; set; }
        internal IPromoCode Promocode { get; set; }

        private BuyerUi TempBuyer { get; set; }
        
        public BooksShop()
        {
            TempBuyer = new BuyerUi();

            this.Books = new WorkWithBooks();
            this.Order = new WorkWithOrder(TempBuyer);
        }

    }
}
