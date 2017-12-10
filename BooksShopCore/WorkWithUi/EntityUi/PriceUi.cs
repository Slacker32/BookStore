using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class PriceUi
    {
        public int PriceId { get; set; }// уникальный идентификатор
        public decimal Price { get; set; }// цена
        public CurrencyUi Currency { get; set; }// валюта
        public CountryUi Country { get; set; }//страна в которой используется данная цена

    }
}
