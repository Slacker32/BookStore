using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal interface IDataRepository<T> : IDisposable
    {
        IList<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
        IList<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
        IList<T> ReadAll(); // чтение всех элементов из хранилища
        void Create(T item);// создание элемента
        T Read(int index); // чтение элемента из хранилища по индексу
        void Update(T item);// обновление элемента
        void Delete(int id);// удаление элемента по индексу
        void AddOrUpdate(T item);//удаление или обновление объекта
        void SaveChanges();//сохранение изменений в хранилище
    }

}
