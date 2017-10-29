using BooksShopCore.WorkWithStorage.EntityStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    internal class BookStoreInitializer:CreateDatabaseIfNotExists<BookStoreContext>
    {

        protected override void Seed(BookStoreContext db)
        {
            AddBooks();
        }
        private void AddBooks()
        {

        }
    }
}
