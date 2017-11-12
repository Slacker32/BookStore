﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("NameBooksTranslate")]
    internal class NameBooksTranslateData // язык наименования книги
    {
        public int Id { get; set; }

        public int? BookDataId { get; set; }// FK на таблицу книг
        public BookData Book { get; set; }// книга

        public string NameBook { get; set; }// название книги

        public int? LanguageDataId { get; set; }// FK на таблицу языков
        public virtual LanguageData Language { get; set; }// язык наименования
    }

}
