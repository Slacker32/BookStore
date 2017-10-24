using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class CurrencyUi // тип данных валюта
    {
        int CurrencyId { get; set; }// уникальный идентификатор валюты
        string CurrencyCode { get; set; }// строковый код валюты по ISO
    }

}
