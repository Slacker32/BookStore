using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Authors")]
    internal class AuthorData // тип данных автор
    {
        public int Id { get; set; }
        public string Name { get; set; }// наименование автора
        public string Year { get; set; }// годы жизни
        public string Info { get; set; }// краткая информация об авторе

        public virtual IList<BookData> Books { get; set; }// книги автора
    }

}
