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
        IBooksShop bookShop;//= new BooksShopCore.BooksShop();
        string Language { get; set; } = "rus";
        string Currency { get; set; } = "byn";

        public HomeController()
        { }
        public HomeController(IBooksShop bookShop)
        {
            this.bookShop = bookShop;
        }

        private async Task<CurrencySite> GetCurrencyList()
        {
            var currencyForView = new CurrencySite();
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
            var currencyForView = await GetCurrencyList();

            var bookList = await bookShop.Books.ShowAllBooksAsync(this.Language, this.Currency);

            var bookListForViews = new List<BookSite>();
            foreach (var book in bookList)
            {
                var bookSite = new BookSite();
                bookSite.BookId = book.BookId;
                bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
                bookSite.Year = book.Year;
                bookSite.Price = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase))?.Price ?? 0;
                bookSite.Format = book.Format.FormatName;
                if (bookSite.Price > 0)
                {
                    bookListForViews.Add(bookSite);
                }
            }
            //ViewBag.Books = bookListForViews;
            Session["CurrentPage"] = "Index";

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
            var currencyForView = GetCurrencyList().Result;

            var bookList = bookShop.Books.FindBooks(searchValue,this.Language, this.Currency);
            var bookListForViews = new List<BookSite>();
            foreach (var book in bookList)
            {
                var bookSite = new BookSite();
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


            var model = new BookCurrencyViewModel() { CurrencyView = currencyForView, BooksView = bookListForViews };
            return View("Index",model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchBookAsync(string searchValue)
        {
            var currencyForView = await GetCurrencyList();

            var bookList = await bookShop.Books.FindBooksAsync(searchValue, this.Language, this.Currency);
            var bookListForViews = new List<BookSite>();
            foreach (var book in bookList)
            {
                var bookSite = new BookSite();
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
            var bookSite = new BookSite();
            bookSite.BookId = book.BookId;
            bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
            bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
            ViewBag.Book = bookSite;

            var preview = bookShop.Preview.GetPreview(id);
            ViewBag.Preview = preview;
            Session["CurrentPage"] = "Preview";
            return View();
        }

        public async Task<ActionResult> PreviewAsync(int id)
        {
            var book = await bookShop.Books.GetBookAsync(id, this.Language, this.Currency);
            var bookSite = new BookSite();
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

            ViewBag.Message = "Книга добавлена в корзину";
            //return Content("<script language='javascript' type='text/javascript'>alert('Книга добавлена в корзину');location.href='/Home/Index';</script>");
            return DialogView();
        }

        public ActionResult Backet()
        {
            if (Session["ListId"] != null)
            {
                var bookStorage = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithBooksStorage();
                var bookListForViews = new List<BookSite>();
                decimal fullAmount = 0;
                var listId = Session["ListId"] as List<int>;
                if (listId != null)
                {
                    foreach (var id in listId)
                    {
                        var book = bookShop.Books.GetBook(id,this.Language, this.Currency);
                        var bookSite = new BookSite();
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

                var amountWithPromoCode = Session["AmountWithPromoCode"] ?? 0m;
                if (((decimal)amountWithPromoCode) >0)
                {
                    ViewBag.AmountOrder = amountWithPromoCode;
                    Session.Remove("AmountWithPromoCode");
                }
            }
            Session["CurrentPage"] = "Backet";
            return View();
        }

        public ActionResult Cancel(int id)
        {
            (Session["ListId"] as List<int>)?.Remove(id);

            //return Content("<script language='javascript' type='text/javascript'>alert('Книга удалена из корзины');location.href='/Home/Backet';</script>");
            ViewBag.Message = "Книга удалена из корзины";
            return DialogView();
        }

        public ActionResult ApplyPromoCode(string promoCode, decimal amount)
        {
            var retValue = bookShop.Promocode.ConsiderPromoCode(promoCode, amount);
            if (retValue.Item1)
            {
                ViewBag.Message = "Промокод применен";
            }
            else
            {
                ViewBag.Message = "Промокод не применен";
            }
            return DialogView();
        }

        public async Task<ActionResult> ApplyPromoCodeAsync(string promoCode,decimal amount)
        {
            var retValue = await bookShop.Promocode.ConsiderPromoCodeAsync(promoCode, amount);
            if (retValue.Item1)
            {
                ViewBag.Message = "Промокод применен";
                Session["AmountWithPromoCode"] = retValue.Item2;
            }
            else
            {
                ViewBag.Message = "Промокод не применен";
            }
            return DialogView();
        }

        public ActionResult ConfirmOrder(string FIO, string Phone,string Address,decimal AmountOrder)
        {
            //return Content("<script language='javascript' type='text/javascript'>alert('Заказ выполнен');location.href='/Home/Backet';</script>");
            ViewBag.Message = "Заказ выполнен";
            return DialogView();
        }

        public ActionResult DialogView()
        {
            return PartialView("DialogView");
        }

        [HttpPost]
        public ActionResult Dialog()
        {
            if (Session["CurrentPage"].ToString() == "Backet")
            {
                return RedirectToAction("Backet");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}