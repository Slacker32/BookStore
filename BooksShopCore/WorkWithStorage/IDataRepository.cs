using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    internal interface IDataRepository<T>
    {
        T Read(int index); // чтение элемента из хранилища по индексу
        IEnumerable<T> ReadAll(); // чтение всех элементов из хранилища
        void Create(T elem);// создание элемента
        void Update(T elem);// обновление элемента
        void Delete(int id);// удаление элемента по индексу
    }

}
