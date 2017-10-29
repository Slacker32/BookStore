using BooksShopCore.WorkWithUi.EntityUi;
using BooksShopCore.WorkWithUi.Logics;
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
    public class BooksShop: IBookShop
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

        public bool Buy(BookUi book)
        {
            var ret = false;
            try
            {
                var tempPurchase = new PurchaseUi();

                //проверка списка на наличие книги
                bool flgAddBook = false;
                if (TempBuyer.ListPurchases?.Count > 0)
                {
                    tempPurchase = TempBuyer.ListPurchases.FirstOrDefault((p) => p.Book.BookId == book.BookId);
                    if (tempPurchase == null)
                    {
                        flgAddBook = true;
                        tempPurchase = new PurchaseUi();
                    }
                    
                }else
                {
                    //первая запись
                    TempBuyer.ListPurchases = new List<PurchaseUi>();
                    flgAddBook = true;
                }

                tempPurchase.Book = book;
                tempPurchase.Amount = book.Price;
                tempPurchase.Buyer = this.TempBuyer;
                tempPurchase.Count++;
                tempPurchase.Currency = book.Currency;
                tempPurchase.Date = DateTime.UtcNow;

                if (flgAddBook)
                {
                    TempBuyer.ListPurchases.Add(tempPurchase);
                }
                ret = true;
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Ошибка при покупке книги: {ex}");
            }
            return ret;
        }

        public bool Reversal(BookUi book)
        {
            var ret = false;
            try
            {
                //проверка списка на наличие книги
                if (TempBuyer.ListPurchases?.Count > 0)
                {
                    var tempPurchase = TempBuyer.ListPurchases.FirstOrDefault((p) => p.Book.BookId == book.BookId);
                    if (tempPurchase == null)
                    {
                        TempBuyer.ListPurchases.RemoveAt(book.BookId);
                    }

                }

                ret = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка при возврате книги: {ex}");
            }
            return ret;
        }
    }
}
