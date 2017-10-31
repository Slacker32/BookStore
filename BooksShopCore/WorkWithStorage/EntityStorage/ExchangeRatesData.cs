using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("ExchangeRates")]
    internal class ExchangeRatesData // тип обменный курс
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }// дата, время задания курса конверсии
        public decimal Rate { get; set; }// курс конверсии


        public int CurrencyDataFromId { get; set; }// FK на таблицу валют
        [ForeignKey("CurrencyDataFromId")]
        public CurrencyData CurrencyFrom { get; set; }// валюта «Из»


        public int CurrencyDataToId { get; set; }// FK на таблицу CurrencyData
        [ForeignKey("CurrencyDataToId")]
        public CurrencyData CurrencyTo { get; set; }// валюта «В»
    }

}
