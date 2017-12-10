using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class BuyerUi // тип данных покупатель
    {
        public int BuyerId { get; set; }// уникальный идентификатор покупателя
        public string FullName { get; set; }// полное имя покупателя
        public string Address { get; set; }// адрес покупателя(может содержать самые разные данные типа почтового индекса, адреса) на основании поля возможно указать место для доставки
        public string Phone { get; set; }// телефон покупателя
        public IList<PurchaseUi> ListPurchases { get; set; }// список покупок пользователя завершенных и нет
        public IList<OrderUi> ListOrders { get; set; }// список заказов пользователя

    }

}
