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
    public class WorkWithAuthorStorage : IDisposable, IWorkWithDataStorage<AuthorUi>
    {
        private IDataRepository<AuthorData> AuthorDataRepository { get; set; }

        public WorkWithAuthorStorage()
        {
            AuthorDataRepository = new GenericRepository<AuthorData>(new BookStoreContext());

        }

        public IList<AuthorUi> ReadAll()
        {
            IList<AuthorUi> ret = null;
            try
            {
                var authorFromStorage = AuthorDataRepository.ReadAll();
                if (authorFromStorage != null)
                {
                    foreach (var item in authorFromStorage)
                    {
                        var author = new AuthorUi()
                        {
                            AuthorId = item.Id,
                            Name = item.Name,
                            Info = item.Info,
                            Year = item.Year
                        };

                        if (ret == null)
                        {
                            ret = new List<AuthorUi>();
                        }
                        ret.Add(author);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения данных по всем авторам из хранилища данных");
                throw;
            }

            return ret;
        }

        public void Create(AuthorUi item)
        {
            try
            {
                if (item != null)
                {
                    var authorData = new AuthorData()
                    {
                        Name = item.Name,
                        Info = item.Info,
                        Year = item.Year
                    };

                    AuthorDataRepository.Create(authorData);
                    AuthorDataRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления автора в хранилище данных");
                throw;
            }
        }

        public AuthorUi Read(int id)
        {
            AuthorUi ret = null;
            try
            {
                var authorFromStorage = AuthorDataRepository.Read(id);
                if (authorFromStorage != null)
                {
                    var author = new AuthorUi()
                    {
                        AuthorId = authorFromStorage.Id,
                        Info = authorFromStorage.Info,
                        Name = authorFromStorage.Name,
                        Year = authorFromStorage.Year
                    };

                    ret = author;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения автора по индексу из хранилища");
                throw;
            }
            return ret;
        }

        public AuthorUi Read(string authorName)
        {
            AuthorUi ret = null;
            try
            {
                var authorFromStorage = AuthorDataRepository.ReadAll().FirstOrDefault(p=>p.Name.Equals(authorName,StringComparison.OrdinalIgnoreCase));
                if (authorFromStorage != null)
                {
                    var author = new AuthorUi()
                    {
                        AuthorId = authorFromStorage.Id,
                        Info = authorFromStorage.Info,
                        Name = authorFromStorage.Name,
                        Year = authorFromStorage.Year
                    };

                    ret = author;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения автора по имени из хранилища");
                throw;
            }
            return ret;
        }

        public void Update(AuthorUi item)
        {
            try
            {
                if (item != null)
                {
                    var updateAuthorData = AuthorDataRepository.Read(item.AuthorId);
                    if (updateAuthorData != null)
                    {
                        updateAuthorData.Info = item.Info;
                        updateAuthorData.Name = item.Name;
                        updateAuthorData.Year = item.Year;
                        AuthorDataRepository.Update(updateAuthorData);
                        AuthorDataRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка обновления данных автора в хранилище");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AuthorDataRepository.Delete(id);
                AuthorDataRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления автора из хранилища по индексу");
                throw;
            }
        }

        public void AddOrUpdate(AuthorUi item)
        {
            try
            {
                if (item != null)
                {
                    var authorData = new AuthorData()
                    {
                        Name = item.Name,
                        Info = item.Info,
                        Year = item.Year
                    };

                    AuthorDataRepository.AddOrUpdate(authorData);
                    AuthorDataRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или обновления автора в хранилище данных");
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
                    AuthorDataRepository.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion

    }
}
