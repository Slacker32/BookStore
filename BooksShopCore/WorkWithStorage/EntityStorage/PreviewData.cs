using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    internal class PreviewData
    {
        public int Id { get; set; }
        public FormatPreviewData Format { get; set; }// формат данных для превью
        public string Path { get; set; }// путь к файлу в котором храниться информация по превью
        public string Data { get; set; }// если поле пути пустое – отображаются данные этого поля

        public int BookDataId { get; set; }// FK на таблицу книг
        public BookData Book { get; set; }// данные по книге для которой применена ценовая политика
    }

}
