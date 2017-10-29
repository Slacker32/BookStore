using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    interface IUnitOfWork
    {
        int SaveChanges();
    }
    interface IQueryEntities //запросы к бд
    {
        IQueryable<T> Query<T>(); // implementation returns Set<T>().AsNoTracking()
        IQueryable<T> EagerLoad<T>(IQueryable<T> queryable, Expression<Func<T, object>> expression); // implementation returns queryable.Include(expression)
    }
    internal interface ICommandEntities<T>: IQueryEntities, IUnitOfWork where T:class//команды к бд
    {
        T Find(params object[] keyValues);
        IQueryable<T> FindMany(); // implementation returns Set<T>() without .AsNoTracking()
        void Create(T entity); // implementation changes Entry(entity).State
        void Update(T entity); // implementation changes Entry(entity).State
        void Delete(T entity); // implementation changes Entry(entity).State
        void Reload(T entity); // implementation invokes Entry(entity).Reload
    }
    public class WorkWithDbStorage<T> : DbContext, ICommandEntities<T> where T:class 
    {

        public WorkWithDbStorage() : base("BookStoreDbConnection")
        {
            
        }

        public virtual T Find(params object[] keyValues)
        {
            return this.Set<T>().Find(keyValues);
        }
        public virtual IQueryable<T> FindMany()
        {
            return this.Set<T>();
        }
        public virtual void Create(T item)
        {
            this.Set<T>().Add(item);
        }
        public virtual void Update(T item)
        {
            this.Entry<T>(item).State = EntityState.Modified;
        }
        public virtual void Delete(T item)
        {
            var item1 = this.Set<T>().Find(item);
            if (item != null)
            {
                this.Set<T>().Remove(item);
            }
        }
        public virtual void Reload(T item)
        {
            this.Entry<T>(item).Reload();
        }

        public IQueryable<T> Query<T>()
        {
            throw new NotImplementedException();
        }
        public IQueryable<T> EagerLoad<T>(IQueryable<T> queryable, Expression<Func<T, object>> expression)
        {
            throw new NotImplementedException();
        }

    }

    public class SomeServiceClass
    {
        //public SomeServiceClass(IQueryEntities<UserEntities> users, ICommandEntities<EstateModuleEntities> estateModule)
        //{
            
        //}
    }

    #region реализация запросов

    //public interface IQuery<TResult>
    //{
    //}
    //public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    //{
    //    TResult Handle(TQuery query);
    //}
    //public interface IQueryProcessor
    //{
    //    TResult Process<TResult>(IQuery<TResult> query);
    //}
    //public class FindUsersBySearchTextQuery : IQuery<User[]>
    //{
    //    public string SearchText { get; set; }

    //    public bool IncludeInactiveUsers { get; set; }
    //}
    //sealed class QueryProcessor : IQueryProcessor
    //{
    //    private readonly Container container;

    //    public QueryProcessor(Container container)
    //    {
    //        this.container = container;
    //    }

    //    [DebuggerStepThrough]
    //    public TResult Process<TResult>(IQuery<TResult> query)
    //    {
    //        var handlerType =
    //            typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

    //        dynamic handler = container.GetInstance(handlerType);

    //        return handler.Handle((dynamic)query);
    //    }
    //}


    //public class UserController : Controller
    //{
    //    private IQueryProcessor queryProcessor;

    //    public UserController(IQueryProcessor queryProcessor)
    //    {
    //        this.queryProcessor = queryProcessor;
    //    }

    //    public ActionResult SearchUsers(string searchString)
    //    {
    //        var query = new FindUsersBySearchTextQuery
    //        {
    //            SearchText = searchString
    //        };

    //        // Note how we omit the generic type argument,
    //        // but still have type safety.
    //        User[] users = this.queryProcessor.Process(query);

    //        return this.View(users);
    //    }
    //}

    #endregion
}
