using BooksShopCore.WorkWithStorage.EntityStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal class BookSqlRepository : IDataRepository<BookData>
    {
        private BookStoreContext db;

        public BookSqlRepository(BookStoreContext context)
        {
            this.db = context;
        }
        public BookData Read(int id)
        {
            return db.Books.Find(id);
        }

        public IEnumerable<BookData> ReadAll()
        {
            return db.Books;
        }

        public void Create(BookData elem)
        {
            this.db.Books.Add(elem);
        }

        public void Update(BookData elem)
        {
            db.Entry(elem).State = EntityState.Modified;
        }

        public void Delete(int index)
        {
            var book = db.Books.Find(index);
            if (book!=null)
            {
                db.Books.Remove(book);
            }
        }
        
    }

}
