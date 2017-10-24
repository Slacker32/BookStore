using BooksShopCore.WorkWithUi.EntityUi;
using BooksShopCore.WorkWithUi.Logics.Preview;
using BooksShopCore.WorkWithUi.Logics.PromoCode;
using BooksShopCore.WorkWithUi.Logics.WorkWithBooks;
using BooksShopCore.WorkWithUi.Logics.WorkWithOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore
{
    public class BooksShop 
    {
        private IWorkWithBooks Books { get; set; }
        private IWorkWithOrder Order { get; set; }
        private IPreview Preview { get; set; }
        private IPromoCode Promocode { get; set; }

        private BuyerUi TempBuyer { get; set; }
        
        public BooksShop()
        {

        }
    }
}
