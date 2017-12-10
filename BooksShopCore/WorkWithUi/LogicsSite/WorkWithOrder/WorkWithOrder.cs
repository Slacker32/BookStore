using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;


namespace BooksShopCore.WorkWithUi.LogicsSite.WorkWithOrder
{
    public class WorkWithOrder : IWorkWithOrder
    {
        internal IDataRepository<PromocodeData> promocodeRepository;
        internal IDataRepository<PurchaseData> purchaseRepository;
        internal IDataRepository<BuyerData> buyerRepository;
        internal IDataRepository<BookData> bookRepository;

        private BuyerUi TempBuyer { get; set; }

        public WorkWithOrder(BuyerUi buyer)
        {
           this.TempBuyer = buyer ?? new BuyerUi();
           this.promocodeRepository=new GenericRepository<PromocodeData>(new BookStoreContext());
        }

        public bool Buy(BookUi book, decimal amount, CurrencyUi currencyUi)
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

                }
                else
                {
                    //первая запись
                    TempBuyer.ListPurchases = new List<PurchaseUi>();
                    flgAddBook = true;
                }

                tempPurchase.Book = book;
                tempPurchase.Amount = amount;
                tempPurchase.Buyer = this.TempBuyer;
                tempPurchase.Count++;
                tempPurchase.Currency = currencyUi.CurrencyCode;
                tempPurchase.Date = DateTime.UtcNow;

                if (flgAddBook)
                {
                    TempBuyer.ListPurchases.Add(tempPurchase);
                }
                ret = true;
            }
            catch (Exception ex)
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
                    if (tempPurchase?.Count > 1)
                    {
                        tempPurchase.Count--;
                    }
                    else
                    {
                        TempBuyer.ListPurchases.Remove(tempPurchase);
                    }


                    ret = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка при возврате книги: {ex}");
            }
            return ret;
        }

        public bool ApplyPromocode(string promocode)
        {
            var ret = false;
            try
            {
                var promo = promocodeRepository.ReadAll().OrderByDescending(p=>p.Date).FirstOrDefault(p=>p.Code.Equals(promocode,StringComparison.OrdinalIgnoreCase));
                if (promo != null)
                {
                    if (promo.Code.Equals(promocode, StringComparison.OrdinalIgnoreCase))
                    {
                        #region  промокод совпал применяем скидку в процентах для всего заказа пользователя

                        if (this.TempBuyer?.ListPurchases != null)
                        {
                            //у покупателя есть заказы применяем скидочный процент
                            var percent = promo.Percent;
                            foreach (var purchase in this.TempBuyer.ListPurchases)
                            {
                                purchase.Amount -= purchase.Amount*percent/100m;
                            }

                            ret = true;
                        }
                        else
                        {
                            throw new ApplicationException($"У покупателя отстутствуют книги предназначенные для покупки");
                        }

                        #endregion
                    }  
                }
                else
                {
                    throw new ApplicationException($"Код {promocode} не применят");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка поиска книги в хранилище данных:{ex}");
            }
            return ret;
        }

        public IList<PurchaseUi> ShowListCurrentPurchases()
        {
            IList<PurchaseUi> ret = null;
            try
            {
                if (this.TempBuyer?.ListPurchases != null)
                {
                    ret = this.TempBuyer.ListPurchases;
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка загрузки списка покупок для текущего покупатеся:{ex}");
            }
            return ret;
        }

        public bool ConfirmationOrder()
        {
            //Возвращает флаг успешности / ошибки принятия заказа, tempBuyer добавляется в хранилище данных, выбранные книги помечаются флагом блокировки, и не отображаются пользователю.
            //Заново проверяется, что наличие и количество выбранных книг не превышает доступного значения.
            //Для каждой книги формируется покупка(Purchase), и записывается в базу.
            //Необходимо учесть что доставка книги на бумаге невозможна на электронный адрес.

            var ret = false;
            try
            {
                //временный покупатель существует и у него есть заказанные книги 
                if (this.TempBuyer?.ListPurchases?.Count > 0)
                {
                    foreach (var pubrchase in this.TempBuyer?.ListPurchases)
                    {
                        var book = pubrchase.Book;
                        #region блокировка заказанных книг

                        var bookData = bookRepository.Read(book.BookId);
                        if (bookData.BooksStorages?.Count > 0)
                        {
                            var bookIsAvailable = bookData.BooksStorages.Sum(p => p.Count - p.CountInBlocked);
                            if (bookIsAvailable < book.Count)
                            {
                                var countBookPurchase = book.Count;
                                int tempBook = 0;
                                while (tempBook < countBookPurchase)
                                {
                                    //указанное количество книг доступно для заказа они будут блокироваться
                                    var flgAdd = false;
                                    foreach (var storage in bookData.BooksStorages)
                                    {
                                        if ((storage.Count - storage.CountInBlocked) >= tempBook)
                                        {
                                            storage.CountInBlocked += tempBook;
                                            flgAdd = true;
                                            break;
                                        }
                                    }
                                    if (!flgAdd)
                                    {
                                        throw new ApplicationException($"На складах нехватает книг");
                                    }
                                    tempBook++;
                                }
                                bookRepository.Update(bookData);
                            }
                            else
                            {
                                var bookTitle = "Untitled";
                                if (book.ListName?.Count > 0)
                                {
                                    bookTitle = book.ListName[0].Name;
                                }

                                throw new ApplicationException($"Выбранное количество{book.Count} книги {bookTitle} недоступно для заказа");
                            }
                        }
                        

                        #endregion
                    }

                }
                else
                {
                    throw new ApplicationException($"Невозможно подтвердить заказ так-как не выбраны книги для покупки");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка при попытке отмены покупки:{ex}");
            }
            return ret;
        }

        public bool CancelPurchaseAfterConfirmation(int purchaseId)
        {
            var ret = false;
            try
            {
                if (this.TempBuyer?.ListPurchases != null)
                {
                    //ret = this.TempBuyer.ListPurchases;
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка при попытке отмены покупки:{ex}");
            }
            return ret;
        }



        public bool IsCompleted(int purchaseId)
        {
            throw new NotImplementedException();
        }

    }
}
