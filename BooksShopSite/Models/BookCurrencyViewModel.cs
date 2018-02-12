using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksShopSite.Models
{
    public class BookCurrencyViewModel
    {
        public Currency CurrencyView { get; set; }
        public IEnumerable<Book> BooksView { get; set; }
    }
}