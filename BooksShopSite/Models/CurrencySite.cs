using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooksShopSite.Models
{
    public class CurrencySite
    {
        public string SelectedCurrency { get; set; }
        public IList<SelectListItem> Currencies { get; set; }
    }
}