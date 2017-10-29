using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class OrderUi// тип данных заказ
    {
        public int OrderId { get; set; }// уникальный идентификатор заказа
        public int BuyerId { get; set; }// уникальный идентификатор покупателя
        public DateTime Date { get; set; }// дата заказа=дате каждой покупки в списке
        public IList<PurchaseUi> ListPurchases { get; set; }// список покупок в заказе

    }
}
