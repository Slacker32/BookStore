using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal sealed class GenericRepository<T> : IDataRepository<T> where T : class
    {
        private readonly DbContext db;
        private readonly DbSet<T> dbSet;

        public GenericRepository(DbContext context)
        {
            this.db = context;
            this.dbSet = context.Set<T>();
        }
        public T Read(int id)
        {
            return this.dbSet.Find(id);
        }

        public IList<T> ReadAll()
        {
            return this.dbSet.ToList();
        }

        public void AddOrUpdate(T item)
        {
            this.dbSet.AddOrUpdate(item);
        }

        public void Create(T item)
        {
            this.dbSet.Add(item);
        }

        public void Update(T item)
        {
            this.db.Entry<T>(item).State = EntityState.Modified;
        }

        public void Delete(int index)
        {
            var item = this.dbSet.Find(index);
            if (item != null)
            {
                this.dbSet.Remove(item);
            }
        }

        public IList<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public IList<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = this.dbSet.AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        #region частичная реализация паттерна очистки
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        private void Dispose(bool disposing)
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
