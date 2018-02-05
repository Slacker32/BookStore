using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal sealed class GenericRepositoryAsync<T, TContext> : IDataRepositoryAsync<T> where T : class 
                                                            where TContext: DbContext,new()
    {

        public async Task<T> ReadAsync(int id)
        {
            T ret;
            using (var db = new TContext())
            {
                var dbSet = db.Set<T>();
                ret = await dbSet.FindAsync(id);
            }
            return ret;
        }

        public async Task<IList<T>> ReadAllAsync()
        {
            IList<T> ret;
            using (var db = new TContext())
            {
                var dbSet = db.Set<T>();
                ret = await dbSet.ToArrayAsync();
            }
            return ret;
        }

        public async Task AddOrUpdateAsync(T item)
        {
            using (var db = new TContext())
            {
                var dbSet=db.Set<T>();
                dbSet.AddOrUpdate(item);
                await db.SaveChangesAsync();

            }
        }

        public async Task CreateAsync(T item)
        {
            using (var db = new TContext())
            {
                var dbSet = db.Set<T>();
                dbSet.Add(item);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T item)
        {
            //using (var db = new TContext())
            //{
                
            //    db.Entry<T>(item).State = EntityState.Modified;
            //    var dbSet = db.Set<T>();
            //    await db.SaveChangesAsync();
            //}

        }

        public async Task DeleteAsync(int index)
        {
            using (var db = new TContext())
            {
                var dbSet = db.Set<T>();
                var item = await dbSet.FindAsync(index);
                if (item != null)
                {
                    dbSet.Remove(item);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            //не имеет смысла
            //using (var db = new TContext())
            //{
            //    await db.SaveChangesAsync();
            //}

        }

        public async Task<IList<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IList<T> ret;
            using (var db = new TContext())
            {
                ret = await Include(db, includeProperties).ToListAsync();
            }
            return ret; 
        }
        public async Task<IList<T>> GetWithIncludeAsync(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IList<T> ret;
            using (var db = new TContext())
            {
                var data = await Include(db, includeProperties).ToListAsync();
                ret = data.Where(predicate).ToList();
                //ret = await query.Where(predicate).AsQueryable().ToListAsync();
            }
            return ret;
        }
        private IQueryable<T> Include(TContext db,params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = db.Set<T>().AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

    }
}
