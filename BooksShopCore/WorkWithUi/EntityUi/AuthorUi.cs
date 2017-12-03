using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage.EntityStorage;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class AuthorUi
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }// наименование автора
        public string Year { get; set; }// годы жизни
        public string Info { get; set; }// краткая информация об авторе

        public override string ToString()
        {
            return $"Id={AuthorId};Name={Name};Year={Year};Info={Info}";
        }
    }
}
