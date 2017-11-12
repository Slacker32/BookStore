using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage
{
    public class WorkWithFormatBookStorage : IDisposable, IWorkWithDataStorage<FormatBookUi>
    {
        private readonly BookStoreContext db;
        private IDataRepository<FormatBookData> FormatBookDataRepository { get; set; }

        public WorkWithFormatBookStorage()
        {
            this.db = new BookStoreContext();
            FormatBookDataRepository = new GenericRepository<FormatBookData>(db);

        }

        public IList<FormatBookUi> ReadAll()
        {
            IList<FormatBookUi> ret = null;
            try
            {
                var formatBookFromStorage = FormatBookDataRepository.ReadAll();
                if (formatBookFromStorage != null)
                {
                    foreach (var item in formatBookFromStorage)
                    {
                        var formatBook = new FormatBookUi()
                        {
                            FormatBookId = item.Id,
                            FormatName = item.FormatName
                        };

                        if (ret == null)
                        {
                            ret = new List<FormatBookUi>();
                        }
                        ret.Add(formatBook);
                    }
                }
            }
            catch (Exception)
            {

                throw new ApplicationException($"Ошибка получения данных");
            }

            return ret;
        }

        public void Create(FormatBookUi item)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи формата в хранилище данных
                    var formatBook = new FormatBookData()
                    {
                        Id = item.FormatBookId,
                        FormatName = item.FormatName
                    };

                    FormatBookDataRepository.Create(formatBook);
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка добавления формата в хранилище данных: {ex}");
            }
        }

        public FormatBookUi Read(int id)
        {
            FormatBookUi ret = null;
            try
            {
                var formatBookStorage = FormatBookDataRepository.Read(id);
                if (formatBookStorage != null)
                {
                    var formatBook = new FormatBookUi()
                    {
                        FormatBookId = formatBookStorage.Id,
                        FormatName = formatBookStorage.FormatName
                    };

                    ret = formatBook;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка получения формата по индексу: {ex}");
            }
            return ret;
        }
        public FormatBookUi Read(string formatName)
        {
            FormatBookUi ret = null;
            try
            {
                var formatBookStorage = FormatBookDataRepository.ReadAll().FirstOrDefault(p => p.FormatName.Equals(formatName, StringComparison.OrdinalIgnoreCase));
                if (formatBookStorage != null)
                {
                    var formatBook = new FormatBookUi()
                    {
                        FormatBookId = formatBookStorage.Id,
                        FormatName = formatBookStorage.FormatName
                    };

                    ret = formatBook;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка получения формата по индексу: {ex}");
            }
            return ret;
        }

        public void Update(FormatBookUi item)
        {
            try
            {
                if (item != null)
                {
                    var formatBookData = FormatBookDataRepository.Read(item.FormatBookId);
                    if (formatBookData != null)
                    {
                        formatBookData.Id = item.FormatBookId;
                        formatBookData.FormatName = item.FormatName;
                        //formatBookData.Book=item.
                        FormatBookDataRepository.Update(formatBookData);
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка изменения формата в хранилище данных: {ex}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                FormatBookDataRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка удаления формата по индексу: {ex}");
            }
        }

        #region частичная реализация паттерна очистки
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
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
