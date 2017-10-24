using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class BookUi // тип данных книга
    {
        int BookId { get; set; }// уникальный идентификатор книги
        string Author { get; set; }//автор книги
        string Name { get; set; }// название книги
        DateTime Year { get; set; }// год издания
        decimal Price { get; set; }// цена
        CurrencyUi Currency { get; set; }// валюта
        int Count { get; set; }// количество книг в наличии(за вычетом блокированных)
    }

}
