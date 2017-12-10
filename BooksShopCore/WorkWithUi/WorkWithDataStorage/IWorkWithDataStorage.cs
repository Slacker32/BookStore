using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.WorkWithDataStorage
{
    public interface IWorkWithDataStorage<T> where T:class
    {
        IList<T> ReadAll();
        void Create(T item);
        T Read(int id);
        T Read(string findStr);
        void Update(T item);
        void Delete(int id);
        void AddOrUpdate(T item);
    }
}
