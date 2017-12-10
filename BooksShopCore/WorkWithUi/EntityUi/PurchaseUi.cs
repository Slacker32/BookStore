using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class PurchaseUi // тип данных покупка
    {
        public int PurchaseId { get; set; }// уникальный идентификатор покупки
        public DateTime Date { get; set; }// дата покупки
        public BuyerUi Buyer { get; set; }// покупатель книги
        public BookUi Book { get; set; }// ид книги
        public int Count { get; set; }// количество купленных/покупаемых экземпляров книги
        public decimal Amount { get; set; }// сумма покупки
        public string Currency { get; set; }// валюта для суммы
        public bool IsGetMoney { get; set; }// флаг получения оплаты
        public bool IsTransferComplete { get; set; }// флаг передачи книги покупателю


    }

}
