using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooksShopSite.Controllers
{
    public class HomeController : Controller
    {
        BooksShopCore.BooksShop bookShop = new BooksShopCore.BooksShop();
        public ActionResult Index()
        {
            var bookList = bookShop.Books.ShowAllBooks("rus", "byn");
            ViewBag.Books = bookList;

            return View();
        }

    }
}