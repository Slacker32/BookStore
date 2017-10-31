using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Currencies")]
    internal class CurrencyData
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }//– строковый код валюты по ISO
    }

}
