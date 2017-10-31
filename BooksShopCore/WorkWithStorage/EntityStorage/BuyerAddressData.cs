using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("BuyersAddress")]
    internal class BuyerAddressData // тип адрес покупателя
    {
        public int Id { get; set; }

        public int BuyerDataId { get; set; }// FK на таблицу покупателей
        public BuyerData Buyer { get; set; }// данные покупателя

        public FormatAdressBuyerData FormatAdressBuyer { get; set; }// формат адреса покупателя
        public string Adress { get; set; }// значение адреса

    }

}
