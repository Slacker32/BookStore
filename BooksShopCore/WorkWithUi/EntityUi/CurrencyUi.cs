using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class CurrencyUi // тип данных валюта
    {
        public int CurrencyId { get; set; }// уникальный идентификатор валюты
        public string CurrencyCode { get; set; }// строковый код валюты по ISO
    }

}
