using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Languages")]
    internal class LanguageData
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }// код языка по спецификации ISO
    }

}
