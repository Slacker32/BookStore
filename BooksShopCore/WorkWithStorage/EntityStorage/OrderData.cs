using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Orders")]
    internal class OrderData // тип заказ
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } //дата заказа

        public int BuyerDataId { get; set; }// FK на таблицу покупателей
        public BuyerData Buyer { get; set; }// данные покупателя

        public virtual IList<PurchaseData> ListPurchases { get; set; }// список покупок в заказе
    }

}
