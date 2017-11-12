using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class CountryUi // тип данных страна
    {
        public int CountryId { get; set; }// уникальный идентификатор 
        public string CountryCode { get; set; }// строковый код страны по ISO
        public string CountryName { get; set; }// название страны
    }
}
