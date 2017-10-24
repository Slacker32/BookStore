using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    internal class CountryData
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }// код страны по спецификации ISO
    }

}
