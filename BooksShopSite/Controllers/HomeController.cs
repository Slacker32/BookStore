using BooksShopCore;
using BooksShopSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BooksShopSite.Controllers
{
    public class HomeController : Controller
    {
        AbstractBooksShop bookShop = new BooksShopCore.BooksShop();
        string Language { get; set; } = "rus";
        string Currency { get; set; } = "byn";

        public HomeController()
        { }
        public HomeController(AbstractBooksShop bookShop)
        {
            this.bookShop = bookShop;
        }

        private async Task<Currency> GetCurrencyList()
        {
            var currencyForView = new Currency();
            #region получение списка валют
            //var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            //var listCurrency = await currency.ReadAllAsync();
            var listCurrency = await bookShop.Currency.GetAllCurrencyAsync();
            if (listCurrency != null)
            {
                var listCurrencyCode = listCurrency.Select(p => p.CurrencyCode);
                currencyForView.SelectedCurrency = this.Currency;
                foreach (var item in listCurrencyCode)
                {
                    if (currencyForView.Currencies == null)
                    {
                        currencyForView.Currencies = new List<SelectListItem>();
                    }
                    currencyForView.Currencies.Add(new SelectListItem { Value = item, Text = item });

                }
            }
            #endregion
            return currencyForView;
        }

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            //var currencyForView = new Currency();
            //#region получение списка валют
            ////var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            ////var listCurrency = await currency.ReadAllAsync();
            //var listCurrency = await bookShop.Currency.GetAllCurrencyAsync();
            //var listCurrencyCode= listCurrency?.Select(p => p.CurrencyCode);
            //currencyForView.SelectedCurrency = this.Currency;
            //foreach (var item in listCurrencyCode)
            //{
            //    if (currencyForView.Currencies == null)
            //    {
            //        currencyForView.Currencies = new List<SelectListItem>();
            //    }
            //    currencyForView.Currencies.Add(new SelectListItem { Value = item, Text = item });

            //}
            //#endregion
            var currencyForView = await GetCurrencyList();

            var bookList = await bookShop.Books.ShowAllBooksAsync(this.Language, this.Currency);

            var bookListForViews = new List<Book>();
            foreach (var book in bookList)
            {
                var bookSite = new Book();
                bookSite.BookId = book.BookId;
                bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
                bookSite.Year = book.Year;
                bookSite.Price = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase)).Price;
                bookSite.Format = book.Format.FormatName;
                if (bookSite.Price > 0)
                {
                    bookListForViews.Add(bookSite);
                }
            }
            //ViewBag.Books = bookListForViews;

            var model = new BookCurrencyViewModel() { CurrencyView = currencyForView, BooksView = bookListForViews };
            return View("Index",model);
        }

        [HttpPost]
        public async Task<ActionResult> SetCurrency(BookCurrencyViewModel model)
        {
            this.Currency = model.CurrencyView.SelectedCurrency;
            return await IndexAsync();
        }

        [HttpPost]
        public ActionResult SearchBook(string searchValue)
        {
            var model = new Currency();
            #region получение списка валют
            var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            var listCurrency = currency.ReadAll()?.Select(p => p.CurrencyCode);
            model.SelectedCurrency = this.Currency;
            if (listCurrency != null)
            {
                foreach (var item in listCurrency)
                {
                    if (model.Currencies == null)
                    {
                        model.Currencies = new List<SelectListItem>();
                    }
                    model.Currencies.Add(new SelectListItem {Value = item, Text = item});
                }
            }
            #endregion

            var bookList = bookShop.Books.FindBooks(searchValue,this.Language, this.Currency);
            var bookListForViews = new List<Book>();
            foreach (var book in bookList)
            {
                var bookSite = new Book();
                bookSite.BookId = book.BookId;
                bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
                bookSite.Year = book.Year;
                var firstOrDefault = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase));
                if (firstOrDefault != null)
                {
                    bookSite.Price = firstOrDefault.Price;
                }
                bookSite.Format = book.Format.FormatName;
                bookListForViews.Add(bookSite);
            }
            ViewBag.Books = bookListForViews;


            return View("Index",model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchBookAsync(string searchValue)
        {
            //var model = new Currency();
            //#region получение списка валют
            //var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            //var listCurrency =await currency.ReadAllAsync();
            //var listCurrencyCode= listCurrency?.Select(p => p.CurrencyCode);
            //model.SelectedCurrency = this.Currency;
            //if (listCurrency != null)
            //{
            //    foreach (var item in listCurrencyCode)
            //    {
            //        if (model.Currencies == null)
            //        {
            //            model.Currencies = new List<SelectListItem>();
            //        }
            //        model.Currencies.Add(new SelectListItem { Value = item, Text = item });
            //    }
            //}
            //#endregion
            var currencyForView = await GetCurrencyList();

            var bookList = await bookShop.Books.FindBooksAsync(searchValue, this.Language, this.Currency);
            var bookListForViews = new List<Book>();
            foreach (var book in bookList)
            {
                var bookSite = new Book();
                bookSite.BookId = book.BookId;
                bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
                bookSite.Year = book.Year;
                var firstOrDefault = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase));
                if (firstOrDefault != null)
                {
                    bookSite.Price = firstOrDefault.Price;
                }
                bookSite.Format = book.Format.FormatName;
                bookListForViews.Add(bookSite);
            }

            //ViewBag.Books = bookListForViews;
            var model = new BookCurrencyViewModel() { CurrencyView = currencyForView, BooksView = bookListForViews };

            return View("Index", model);
        }

        public ActionResult Preview(int id)
        {
            var book = bookShop.Books.GetBook(id, this.Language, this.Currency);
            var bookSite = new Book();
            bookSite.BookId = book.BookId;
            bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
            bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
            ViewBag.Book = bookSite;

            var preview = bookShop.Preview.GetPreview(id);
            ViewBag.Preview = preview;
            return View();
        }

        public async Task<ActionResult> PreviewAsync(int id)
        {
            var book = await bookShop.Books.GetBookAsync(id, this.Language, this.Currency);
            var bookSite = new Book();
            bookSite.BookId = book.BookId;
            bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
            bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
            ViewBag.Book = bookSite;

            var preview = await bookShop.Preview.GetPreviewAsync(id);
            ViewBag.Preview = preview;
            return View("Preview");
        }

        public ActionResult Buy(int id)
        {
            // если книга отображена на сайте значит она есть в хранилище и имеет id
            if (Session["ListId"] == null)
            {
                Session["ListId"] = new List<int>();
            }
            (Session["ListId"] as List<int>)?.Add(id);


            //return Content("<script language='javascript' type='text/javascript'>alert('Книга добавлена в корзину');location.href='/Home/Index';</script>");
            return DialogView();
        }

        public ActionResult Backet()
        {
            if (Session["ListId"] != null)
            {
                var bookStorage = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithBooksStorage();
                var bookListForViews = new List<Book>();
                decimal fullAmount = 0;
                var listId = Session["ListId"] as List<int>;
                if (listId != null)
                {
                    foreach (var id in listId)
                    {
                        var book = bookShop.Books.GetBook(id,this.Language, this.Currency);
                        var bookSite = new Book();
                        bookSite.BookId = book.BookId;
                        bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                        bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
                        bookSite.Year = book.Year;
                        var firstOrDefault = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase));
                        if (firstOrDefault != null)
                        {
                            bookSite.Price = firstOrDefault.Price;
                        }
                        bookSite.Format = book.Format.FormatName;
                        fullAmount += bookSite.Price;
                        bookListForViews.Add(bookSite);
                    }
                }
                ViewBag.Books = bookListForViews;
                ViewBag.AmountOrder = fullAmount;
            }
            
            return View();
        }

        public ContentResult Cancel(int id)
        {
            (Session["ListId"] as List<int>)?.Remove(id);

            return Content("<script language='javascript' type='text/javascript'>alert('Книга удалена из корзины');location.href='/Home/Backet';</script>");
        }

        public ContentResult ApplyPromoCode(string PromoCode)
        {

            return Content("<script language='javascript' type='text/javascript'>alert('Промокод применен');location.href='/Home/Backet';</script>");
        }

        public ContentResult ConfirmOrder(string FIO, string Phone,string Address)
        {

            return Content("<script language='javascript' type='text/javascript'>alert('Заказ выполнен');location.href='/Home/Backet';</script>");
        }

        public ActionResult DialogView()
        {
            return PartialView("DialogView");
        }

        [HttpPost]
        public ActionResult Dialog()
        {
            return RedirectToAction("Index");
        }
    }
}