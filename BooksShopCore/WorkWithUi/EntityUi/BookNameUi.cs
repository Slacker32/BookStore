using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class BookNameUi
    {
        public int BookNameId { get; set; }
        public string Name { get; set; }// наименование книги
        public LanguageUi LanguageBookCode { get; set; }// код языка книги
    }
}
