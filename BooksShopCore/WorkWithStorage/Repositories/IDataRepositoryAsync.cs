using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.Repositories
{
    internal interface IDataRepositoryAsync<T>
    {
        Task<IList<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IList<T>> GetWithIncludeAsync(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IList<T>> ReadAllAsync(); // чтение всех элементов из хранилища
        Task CreateAsync(T item);// создание элемента
        Task<T> ReadAsync(int index); // чтение элемента из хранилища по индексу
        Task UpdateAsync(T item);// обновление элемента
        Task DeleteAsync(int id);// удаление элемента по индексу
        Task AddOrUpdateAsync(T item);//удаление или обновление объекта
    }
}
