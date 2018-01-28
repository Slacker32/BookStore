﻿using BooksShopSite.Models;
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
        BooksShopCore.BooksShop bookShop = new BooksShopCore.BooksShop();
        string Language { get; set; } = "rus";
        string Currency { get; set; } = "byn";

        //public ActionResult Index()
        //{
        //    var model = new Currency();
        //    #region получение списка валют
        //    var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
        //    var listCurrency = currency.ReadAll()?.Select(p => p.CurrencyCode);
        //    model.SelectedCurrency = this.Currency;
        //    foreach (var item in listCurrency)
        //    {
        //        if (model.Currencies == null)
        //        {
        //            model.Currencies = new List<SelectListItem>();
        //        }
        //        model.Currencies.Add(new SelectListItem { Value = item, Text = item });
        //        //if (this.Language.Equals(item,StringComparison.OrdinalIgnoreCase))
        //        //{
        //        //    model.SelectedCurrency = item;
        //        //}
        //    }
        //    #endregion

        //    var bookList = bookShop.Books.ShowAllBooks(this.Language, this.Currency);

        //    var bookListForViews = new List<Book>();
        //    foreach (var book in bookList)
        //    {
        //        var bookSite = new Book();
        //        bookSite.BookId = book.BookId;
        //        bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
        //        bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(this.Language, StringComparison.OrdinalIgnoreCase))?.Name;
        //        bookSite.Year = book.Year;
        //        bookSite.Price = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(this.Currency, StringComparison.OrdinalIgnoreCase)).Price;
        //        bookSite.Format = book.Format.FormatName;
        //        bookListForViews.Add(bookSite);
        //    }
        //    ViewBag.Books = bookListForViews;

        //    return View(model);
        //}

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var model = new Currency();
            #region получение списка валют
            var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            var listCurrency = await currency.ReadAllAsync();
            var listCurrencyCode= listCurrency?.Select(p => p.CurrencyCode);
            model.SelectedCurrency = this.Currency;
            foreach (var item in listCurrencyCode)
            {
                if (model.Currencies == null)
                {
                    model.Currencies = new List<SelectListItem>();
                }
                model.Currencies.Add(new SelectListItem { Value = item, Text = item });
                //if (this.Language.Equals(item,StringComparison.OrdinalIgnoreCase))
                //{
                //    model.SelectedCurrency = item;
                //}
            }
            #endregion

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
                bookListForViews.Add(bookSite);
            }
            ViewBag.Books = bookListForViews;

            return View("Index",model);
        }

        //[HttpPost]
        //public ActionResult Index(Currency model)
        //{
        //    this.Currency = model.SelectedCurrency;
        //    return Index();
        //}

        [HttpPost]
        public async Task<ActionResult> SetCurrency(Currency model)
        {
            this.Currency = model.SelectedCurrency;
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
            var model = new Currency();
            #region получение списка валют
            var currency = new BooksShopCore.WorkWithUi.WorkWithDataStorage.WorkWithCurrencyStorage();
            var listCurrency =await currency.ReadAllAsync();
            var listCurrencyCode= listCurrency?.Select(p => p.CurrencyCode);
            model.SelectedCurrency = this.Currency;
            if (listCurrency != null)
            {
                foreach (var item in listCurrencyCode)
                {
                    if (model.Currencies == null)
                    {
                        model.Currencies = new List<SelectListItem>();
                    }
                    model.Currencies.Add(new SelectListItem { Value = item, Text = item });
                }
            }
            #endregion

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
            ViewBag.Books = bookListForViews;


            return View("Index", model);
        }

        public ContentResult Buy(int id)
        {
            if (Session["ListId"] == null)
            {
                Session["ListId"] = new List<int>();
            }
            (Session["ListId"] as List<int>)?.Add(id);
            

            return Content("<script language='javascript' type='text/javascript'>alert('Книга добавлена в корзину');location.href='/Home/Index';</script>");
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

    }
}