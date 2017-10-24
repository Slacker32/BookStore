using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class OrderUi// тип данных заказ
    {
        int OrderId { get; set; }// уникальный идентификатор заказа
        int BuyerId { get; set; }// уникальный идентификатор покупателя
        DateTime Date { get; set; }// дата заказа=дате каждой покупки в списке
        IList<PurchaseUi> ListPurchases { get; set; }// список покупок в заказе


    }
}
