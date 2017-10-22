using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    public class OrderData // тип заказ
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } //дата заказа

        public int BuyerDataId { get; set; }// FK на таблицу покупателей
        public BuyerData Buyer { get; set; }// данные покупателя

        public IList<PurchaseData> ListPurchases { get; set; }// список покупок в заказе
    }

}
