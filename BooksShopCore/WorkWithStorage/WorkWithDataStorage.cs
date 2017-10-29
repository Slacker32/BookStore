using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    public class WorkWithDataStorage:IDisposable
    {
        private readonly BookStoreContext db;
        private IDataRepository<BookData> BookRepositiry { get; set; }
        private IDataRepository<PurchaseData> PurchaseRepositiry { get; set; }
        private IDataRepository<BuyerData> BuyerRepositiry { get; set; }

        internal IList<BookUi> Books { get; set; }

        public WorkWithDataStorage()
        {
            db = new BookStoreContext();
            //Books = new BookSqlRepository(db);
            //Purchases = new PurchaseSqlRepository(db);
            //Buyers = new BuyerSqlRepository(db);
            BookRepositiry = new GenericRepository<BookData>(db);
            PurchaseRepositiry = new GenericRepository<PurchaseData>(db);
            BuyerRepositiry = new GenericRepository<BuyerData>(db);

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
