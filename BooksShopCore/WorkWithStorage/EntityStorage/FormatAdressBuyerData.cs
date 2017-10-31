using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("FormatsAdressBuyer")]
    internal class FormatAdressBuyerData // тип формат адреса покупателя
    {
        public int Id { get; set; }
        public string FormatAdressName { get; set; }// формат адреса(физический, электронный)
    }

}
