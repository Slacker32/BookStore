using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Purchases")]
    internal class PurchaseData // тип данных покупка(подразумевается покупка только одной книги)
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }//        дата покупки

        public int BuyerId { get; set; }// FK на таблицу Buyer
        public BuyerData Buyer { get; set; }// данные покупателя

        public int BookId { get; set; }// FK на таблицу Book
        public BookData Book { get; set; }// данные по книге

        public int? OrderDataId { get; set; }// FK на таблицу OrderData
        public virtual OrderData Order { get; set; }// данные по заказу

        public int Count { get; set; }// количество купленных/ покупаемых экземпляров книги
        public decimal Amount { get; set; }// сумма покупки

        public int CurrencyDataId { get; set; }// FK на таблицу CurrencyData
        public CurrencyData Currency { get; set; }// валюта для суммы

        public bool IsGetMoney { get; set; }// флаг получения оплаты
        public bool IsTransferComplete { get; set; }// флаг передачи книги покупателю
    }

}
