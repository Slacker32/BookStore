using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    public class CurrencyData
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }//– строковый код валюты по ISO
    }

}
