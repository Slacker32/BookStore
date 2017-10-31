using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Storages")]
    internal class StorageData // склад
    {
        public int Id { get; set; }
        public string NameStorage { get; set; }// название хранилища

        public IList<BooksStorage> BooksStorages { get; set; }// список сопоставлений

    }

}
