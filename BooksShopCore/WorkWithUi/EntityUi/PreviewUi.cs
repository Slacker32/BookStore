using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class PreviewUi // тип данных превью
    {
        public int PreviewId { get; set; }// уникальный идентификатор
        public string BookId { get; set; }// ид книги на которую будет превью
        public string Path { get; set; }// путь к файлу с превью

    }
}
