using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BooksShopSite.Models
{
    public class Book
    {
        public int BookId { get; set; }// уникальный идентификатор книги
        public string Authors { get; set; }//авторы книги через точку с запятой
        public string Name { get; set; }// название книги
        public DateTime Year { get; set; }// год издания
        public decimal Price { get; set; } //цена книги

        public string Format { get; set; }//формат книги
        public int Count { get; set; }// количество книг в наличии(за вычетом блокированных)
    }
}