using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class PurchaseUi // тип данных покупка
    {
        int PurchaseId { get; set; }// уникальный идентификатор покупки
        DateTime Date { get; set; }// дата покупки
        BuyerUi Buyer { get; set; }// покупатель книги
        BookUi Book { get; set; }// ид книги
        int Count { get; set; }// количество купленных/покупаемых экземпляров книги
        decimal Amount { get; set; }// сумма покупки
        CurrencyUi Currency { get; set; }// валюта для суммы
        bool IsGetMoney { get; set; }// флаг получения оплаты
        bool IsTransferComplete { get; set; }// флаг передачи книги покупателю
    }

}
