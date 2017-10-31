using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("BooksStorages")]
    internal class BooksStorage // сопоставление складов и книг
    {
        public int Id { get; set; }

        public int StorageDataId { get; set; }// FK на таблицу склад
        public StorageData Storage { get; set; }// название склада

        public int BookId { get; set; }// FK на таблицу книг
        public BookData Book { get; set; }// название книги 

        public int Count { get; set; }//количество экземпляров книги в наличии
        public int CountInBlocked { get; set; }// количество экземпляров книги заблокированных для доставки
    }

}
