using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    internal class StorageData // склад
    {
        public int Id { get; set; }
        public string NameStorage { get; set; }// название хранилища

        public IList<BooksStorage> BooksStorages { get; set; }// список сопоставлений

    }

}
