using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    public class BuyerData // тип данных покупатель
    {
        public int Id { get; set; }
        public string FullName { get; set; }// полное имя покупателя
        //public BuyerAddressData BuyerAddress { get; set; }// адрес покупателя(может содержать самые разные данные типа почтового индекса, адреса) на основании поля возможно указать место для доставки
        public string Phone { get; set; }// телефон покупателя

        public IList<BuyerAddressData> BuyerAddress { get; set; }// адрес покупателя(может содержать самые разные данные типа почтового индекса, адреса) на основании поля возможно указать место для доставки
        public IList<PurchaseData> ListPurchases { get; set; }// список покупок пользователя завершенных и нет
        public IList<OrderData> ListOrders { get; set; }// список заказов пользователя

    }

}
