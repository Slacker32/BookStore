using BooksShopCore.WorkWithStorage.EntityStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal class PurchaseSqlRepository : IDataRepository<PurchaseData>
    {
        private readonly BookStoreContext db;

        public PurchaseSqlRepository(BookStoreContext context)
        {
            this.db = context;
        }
        public PurchaseData Read(int id)
        {
            return db.Purchases.Find(id);
        }

        public IEnumerable<PurchaseData> ReadAll()
        {
            return db.Purchases;
        }

        public void Create(PurchaseData elem)
        {
            this.db.Purchases.Add(elem);
        }

        public void Update(PurchaseData elem)
        {
            db.Entry(elem).State = EntityState.Modified;
        }

        public void Delete(int index)
        {
            var purchase = db.Purchases.Find(index);
            if (purchase != null)
            {
                db.Purchases.Remove(purchase);
            }
        }




    }
}
