using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class BuyerUi // тип данных покупатель
    {
        int BuyerId { get; set; }// уникальный идентификатор покупателя
        string FullName { get; set; }// полное имя покупателя
        string Address { get; set; }// адрес покупателя(может содержать самые разные данные типа почтового индекса, адреса) на основании поля возможно указать место для доставки
        string Phone { get; set; }// телефон покупателя
        IList<PurchaseUi> ListPurchases { get; set; }// список покупок пользователя завершенных и нет
    }

}
