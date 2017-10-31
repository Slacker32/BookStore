using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Books")]
    internal class BookData // тип данных книга
    {
        public int Id { get; set; }
        public IList<NameBooksTranslateData> NameBooksTranslates { get; set; }// названия книги на разных языках
        public DateTime Year { get; set; }// год издания

        public IList<FormatBookData> FormatBook { get; set; }// формат книги
        public IList<BooksStorage> BooksStorages { get; set; }// список сопоставлений
        public IList<AuthorData> Authors { get; set; }// авторы книги
        public IList<PricePolicyData> PricePolicy { get; set; }// данные по цене и количеству книг
        public IList<PreviewData> Preview { get; set; }// данные по фрагменту книги для показа
    }

}
