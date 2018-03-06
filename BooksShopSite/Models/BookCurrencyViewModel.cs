using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksShopSite.Models
{
    public class BookCurrencyViewModel
    {
        public CurrencySite CurrencyView { get; set; }
        public IEnumerable<BookSite> BooksView { get; set; }
    }
}