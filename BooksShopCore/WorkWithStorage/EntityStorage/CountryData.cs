using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Countries")]
    internal class CountryData
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }// код страны по спецификации ISO
        public string CountryName { get; set; }// название страны по спецификации ISO
    }

}
