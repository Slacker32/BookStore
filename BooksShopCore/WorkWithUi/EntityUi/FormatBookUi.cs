using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class FormatBookUi // формат книги
    {
        public int FormatBookId { get; set; }// уникальный идентификатор валюты
        public string FormatName { get; set; }// формат данных книги(paper, pdf, fb2, doc, rtf, txt)

    }
}
