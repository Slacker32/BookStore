using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    internal class FormatBookData
    {
        public int Id { get; set; }
        public string FormatName { get; set; }// формат данных книги(paper, pdf, fb2, doc, rtf, txt)

        public int BookDataId { get; set; }// FK на таблицу книг
        public BookData Book { get; set; }// книга
    }

}
