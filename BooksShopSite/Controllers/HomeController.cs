using BooksShopSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BooksShopSite.Controllers
{
    public class HomeController : Controller
    {
        BooksShopCore.BooksShop bookShop = new BooksShopCore.BooksShop();
        string Language { get; set; } = "rus";
        string Currency { get; set; } = "byn";

        public ActionResult Index()
        {
            #region получение списка валют
            var currency = new BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage.WorkWithCurrencyStorage();
            var listCurrency = currency.ReadAll().Select(p => p.CurrencyCode);
            ViewBag.Currency = listCurrency;
            #endregion

            var bookList = bookShop.Books.ShowAllBooks(this.Language, this.Currency);

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
                bookListForViews.Add(bookSite);
            }
            ViewBag.Books = bookListForViews;

            return View();
        }

        public ContentResult Buy(int id)
        {
            if (Session["ListId"] == null)
            {
                Session["ListId"] = new List<int>();
            }
            (Session["ListId"] as List<int>).Add(id);
            

            return Content("<script language='javascript' type='text/javascript'>alert('Книга добавлена в корзину');location.href='/Home/Index';</script>");
        }

        public ActionResult Preview()
        {
            return View();
        }

        public ActionResult Backet()
        {
            if (Session["ListId"] != null)
            {
                var bookStorage = new BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage.WorkWithBooksStorage();
                var bookListForViews = new List<Book>();
                decimal fullAmount = 0;
                var listId = Session["ListId"] as List<int>;
                if (listId != null)
                {
                    foreach (var id in listId)
                    {
                        var book = bookStorage.Read(id);
                        var bookSite = new Book();
                        bookSite.BookId = book.BookId;
                        bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                        bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
                        bookSite.Year = book.Year;
                        bookSite.Price = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase)).Price;
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


    }
}