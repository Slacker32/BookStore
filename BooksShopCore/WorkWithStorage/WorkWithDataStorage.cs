using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    public class WorkWithDataStorage:IDisposable
    {
        private BookStoreContext db;
        private IDataRepository<BookData> Books;
        private IDataRepository<PurchaseData> Purchases;
        private IDataRepository<BuyerData> Buyers;

        public WorkWithDataStorage()
        {
            db = new BookStoreContext();
            Books = new BookSqlRepository(db);
            Purchases = new PurchaseSqlRepository(db);
            Buyers = new BuyerSqlRepository(db);

        }

        public void Save()
        {
            db.SaveChanges();
        }

        #region частичная реализация паттерна очистки
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
