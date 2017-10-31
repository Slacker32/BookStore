using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("PricePolicies")]
    internal class PricePolicyData // тип ценовая политика для разных стран
    {
        public int Id { get; set; }

        public int BookDataId { get; set; }// FK на таблицу книг
        public BookData Book { get; set; }// данные по книге для которой применена ценовая политика

        public decimal Price { get; set; }// цена книги

        public int CurrencyDataId { get; set; }// FK на таблицу валют
        public CurrencyData Currency { get; set; }// валюта для цены

        public int CountryDataId { get; set; }// FK на таблицу стран
        public CountryData Country { get; set; }// страна для которой применена ценовая политика
    }

}
