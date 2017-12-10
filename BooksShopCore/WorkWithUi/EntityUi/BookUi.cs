using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class BookUi // тип данных книга
    {
        public int BookId { get; set; }// уникальный идентификатор книги
        public List<AuthorUi> Authors { get; set; }//авторы книги
        public List<BookNameUi> ListName { get; set; }// название книги на разных языках
        public DateTime Year { get; set; }// год издания
        public List<PriceUi> ListPrice { get; set; } //список цен книги

        public FormatBookUi Format { get; set; }//формат книги
        public int Count { get; set; }// количество книг в наличии(за вычетом блокированных)

        public bool FindAuthor(string authorName)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(authorName) && (this.Authors != null))
            {
                if (this.Authors.Any(author => author.Name.Equals(authorName, StringComparison.OrdinalIgnoreCase)))
                {
                    ret = true;
                }
            }

            return ret;
        }
        public bool FindName(string nameBook)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(nameBook) && (this.ListName != null))
            {
                if (this.ListName.Any(book => book.Name.Equals(nameBook, StringComparison.OrdinalIgnoreCase)))
                {
                    ret = true;
                }
            }

            return ret;
        }


    }

}
