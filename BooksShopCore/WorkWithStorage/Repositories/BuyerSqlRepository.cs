using BooksShopCore.WorkWithStorage.EntityStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal class BuyerSqlRepository : IDataRepository<BuyerData>
    {
        private BookStoreContext db;

        public BuyerSqlRepository(BookStoreContext context)
        {
            this.db = context;
        }
        public BuyerData Read(int id)
        {
            return db.Buyers.Find(id);
        }

        public IEnumerable<BuyerData> ReadAll()
        {
            return db.Buyers;
        }

        public void Create(BuyerData elem)
        {
            this.db.Buyers.Add(elem);
        }

        public void Update(BuyerData elem)
        {
            db.Entry(elem).State = EntityState.Modified;
        }

        public void Delete(int index)
        {
            var buyer = db.Buyers.Find(index);
            if (buyer != null)
            {
                db.Buyers.Remove(buyer);
            }
        }

    }
}
