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
        public string Author { get; set; }//автор книги
        public string Name { get; set; }// название книги
        public DateTime Year { get; set; }// год издания
        public decimal Price { get; set; }// цена
        public CurrencyUi Currency { get; set; }// валюта
        public FormatBookUi Format { get; set; }//формат книги
        public int Count { get; set; }// количество книг в наличии(за вычетом блокированных)
    }

}
