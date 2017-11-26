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
        private IDataRepository<LanguageData> LanguageRepository { get; set; }

        public WorkWithLanguageStorage()
        {
            LanguageRepository = new GenericRepository<LanguageData>(new BookStoreContext());

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
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения данных по всем языкам из хранилища");
                throw;
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
                    LanguageRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления языка в хранилище данных");
                throw;
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
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения языка из хранилища данных по его индексу");
                throw;
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
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения языка из хранилища данных по его коду");
                throw;
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
                        LanguageRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка изменения данных по языку в хранилище данных");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                LanguageRepository.Delete(id);
                LanguageRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления языка в хранилище данных");
                throw;
            }
        }

        public void AddOrUpdate(LanguageUi item)
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

                    LanguageRepository.AddOrUpdate(languageData);
                    LanguageRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или обновления языка в хранилище данных");
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
                    LanguageRepository.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
