using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopCore.WorkWithUi.WorkWithDataStorage
{
    public class WorkWithFormatBookStorage : IDisposable, IWorkWithDataStorage<FormatBookUi>
    {
        private IDataRepository<FormatBookData> FormatBookDataRepository { get; set; }
        private IDataRepositoryAsync<FormatBookData> FormatBookDataRepositoryAsync { get; set; }

        public WorkWithFormatBookStorage()
        {
            FormatBookDataRepository = new GenericRepository<FormatBookData>(new BookStoreContext());
            FormatBookDataRepositoryAsync = new GenericRepositoryAsync<FormatBookData,BookStoreContext>();

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
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения всех форматов книг из хранилища");
                throw;
            }

            return ret;
        }
        public async Task<IList<FormatBookUi>> ReadAllAsync()
        {
            IList<FormatBookUi> ret = null;
            try
            {
                var formatBookFromStorage = await FormatBookDataRepositoryAsync.ReadAllAsync();
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
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения всех форматов книг из хранилища");
                throw;
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
                    FormatBookDataRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления формата книги в хранилище");
                throw;
            }
        }
        public async Task CreateAsync(FormatBookUi item)
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

                    await FormatBookDataRepositoryAsync.CreateAsync(formatBook);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления формата книги в хранилище");
                throw;
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
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения формата книги из хранилища по индексу");
                throw;
            }
            return ret;
        }
        public async Task<FormatBookUi> ReadAsync(int id)
        {
            FormatBookUi ret = null;
            try
            {
                var formatBookStorage = await FormatBookDataRepositoryAsync.ReadAsync(id);
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
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения формата книги из хранилища по индексу");
                throw;
            }
            return ret;
        }

        public FormatBookUi Read(string bookName)
        {
            FormatBookUi ret = null;
            try
            {
                var formatBookStorage = FormatBookDataRepository.ReadAll().FirstOrDefault(p => p.Book.NameBooksTranslates.FirstOrDefault(p2 => p2.NameBook.Equals(bookName, StringComparison.OrdinalIgnoreCase)) != null);
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
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения формата книги из хранилища по названию книги");
                throw;
            }
            return ret;
        }
        public async Task<FormatBookUi> ReadAsync(string bookName)
        {
            FormatBookUi ret = null;
            try
            {
                var formatBookStorageList = await FormatBookDataRepositoryAsync.ReadAllAsync();
                var formatBookStorage =  formatBookStorageList.FirstOrDefault(p => p.Book.NameBooksTranslates.FirstOrDefault(p2 => p2.NameBook.Equals(bookName, StringComparison.OrdinalIgnoreCase)) != null);
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
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения формата книги из хранилища по названию книги");
                throw;
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
                        FormatBookDataRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка обновления формата книги из хранилища");
                throw;
            }
        }
        public async Task UpdateAsync(FormatBookUi item)
        {
            try
            {
                if (item != null)
                {
                    var formatBookData = await FormatBookDataRepositoryAsync.ReadAsync(item.FormatBookId);
                    if (formatBookData != null)
                    {
                        formatBookData.Id = item.FormatBookId;
                        formatBookData.FormatName = item.FormatName;
                        //formatBookData.Book=item.
                        await FormatBookDataRepositoryAsync.AddOrUpdateAsync(formatBookData);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка обновления формата книги из хранилища");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                FormatBookDataRepository.Delete(id);
                FormatBookDataRepository.SaveChanges();
            }
            catch (Exception ex)
            {

                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления формата книги из хранилища");
                throw;
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                await FormatBookDataRepositoryAsync.DeleteAsync(id);
            }
            catch (Exception ex)
            {

                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления формата книги из хранилища");
                throw;
            }
        }

        public void AddOrUpdate(FormatBookUi item)
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

                    FormatBookDataRepository.AddOrUpdate(formatBook);
                    FormatBookDataRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или изменения формата книги в хранилище");
                throw;
            }
        }
        public async Task AddOrUpdateAsync(FormatBookUi item)
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

                    await FormatBookDataRepositoryAsync.AddOrUpdateAsync(formatBook);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или изменения формата книги в хранилище");
                throw;
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
                    FormatBookDataRepository.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion

    }
}
