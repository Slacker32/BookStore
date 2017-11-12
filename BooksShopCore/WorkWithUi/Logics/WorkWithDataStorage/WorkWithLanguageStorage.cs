using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage
{
    public class WorkWithLanguageStorage : IDisposable, IWorkWithDataStorage<LanguageUi>
    {
        private readonly BookStoreContext db;
        private IDataRepository<LanguageData> LanguageRepository { get; set; }

        public WorkWithLanguageStorage()
        {
            this.db = new BookStoreContext();
            LanguageRepository = new GenericRepository<LanguageData>(db);

        }

        public IList<LanguageUi> ReadAll()
        {
            IList<LanguageUi> ret = null;
            try
            {
                var languageFromStorage = LanguageRepository.ReadAll();
                if (languageFromStorage != null)
                {
                    foreach (var item in languageFromStorage)
                    {
                        var language = new LanguageUi()
                        {
                            LanguageId = item.Id,
                            LanguageCode = item.LanguageCode,
                            LanguageName = item.LanguageName
                        };

                        if (ret == null)
                        {
                            ret = new List<LanguageUi>();
                        }
                        ret.Add(language);
                    }
                }
            }
            catch (Exception)
            {

                throw new ApplicationException($"Ошибка получения данных");
            }

            return ret;
        }

        public void Create(LanguageUi item)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в валюты в хранилище данных
                    var languageData = new LanguageData()
                    {
                        LanguageCode = item.LanguageCode,
                        LanguageName = item.LanguageName
                    };

                    LanguageRepository.Create(languageData);
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка добавления языка в хранилище данных: {ex}");
            }
        }

        public LanguageUi Read(int id)
        {
            LanguageUi ret = null;
            try
            {
                var languageFromStorage = LanguageRepository.Read(id);
                if (languageFromStorage != null)
                {
                    var language = new LanguageUi()
                    {
                        LanguageId = languageFromStorage.Id,
                        LanguageCode = languageFromStorage.LanguageCode,
                        LanguageName = languageFromStorage.LanguageName
                    };

                    ret = language;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка получения языка по индексу: {ex}");
            }
            return ret;
        }

        public LanguageUi Read(string languageCode)
        {
            LanguageUi ret = null;
            try
            {
                var languageFromStorage = LanguageRepository.ReadAll().FirstOrDefault(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                if (languageFromStorage != null)
                {
                    var language = new LanguageUi()
                    {
                        LanguageId = languageFromStorage.Id,
                        LanguageCode = languageFromStorage.LanguageCode,
                        LanguageName = languageFromStorage.LanguageName
                    };

                    ret = language;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка получения языка по коду: {ex}");
            }
            return ret;
        }

        public void Update(LanguageUi item)
        {
            try
            {
                if (item != null)
                {
                    var updateLanguageData = LanguageRepository.Read(item.LanguageId);
                    if (updateLanguageData != null)
                    {
                        updateLanguageData.LanguageCode = item.LanguageCode;
                        updateLanguageData.LanguageName = item.LanguageName;
                        LanguageRepository.Update(updateLanguageData);
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка изменения языка в хранилище данных: {ex}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                LanguageRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка удаления языка по индексу: {ex}");
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
