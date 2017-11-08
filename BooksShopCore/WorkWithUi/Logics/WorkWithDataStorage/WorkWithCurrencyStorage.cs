using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage
{
    public class WorkWithCurrencyStorage : IDisposable
    {
        private readonly BookStoreContext db;
        private IDataRepository<CurrencyData> CurrencyRepository { get; set; }

        public WorkWithCurrencyStorage()
        {
            this.db = new BookStoreContext();
            CurrencyRepository = new GenericRepository<CurrencyData>(db);

        }

        public IList<CurrencyUi> ReadAll()
        {
            IList<CurrencyUi> ret = null;
            try
            {
                var currencyFromStorage = CurrencyRepository.ReadAll();
                if (currencyFromStorage != null)
                {
                    foreach (var item in currencyFromStorage)
                    {
                        var currency = new CurrencyUi()
                        {
                            CurrencyId = item.Id,
                            CurrencyCode = item.CurrencyCode
                        };

                        if (ret == null)
                        {
                            ret = new List<CurrencyUi>();
                        }
                        ret.Add(currency);
                    }
                }
            }
            catch (Exception)
            {

                throw new ApplicationException($"Ошибка получения данных");
            }

            return ret;
        }

        public void Create(CurrencyUi item)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в валюты в хранилище данных
                    var currencyData = new CurrencyData()
                    {
                        CurrencyCode = item.CurrencyCode
                    };

                    CurrencyRepository.Create(currencyData);
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка добавления валюты в хранилище данных: {ex}");
            }
        }

        public CurrencyUi Read(int id)
        {
            CurrencyUi ret = null;
            try
            {
                var currencyFromStorage = CurrencyRepository.Read(id);
                if (currencyFromStorage != null)
                {
                    var currency = new CurrencyUi()
                    {
                        CurrencyId = currencyFromStorage.Id,
                        CurrencyCode = currencyFromStorage.CurrencyCode
                    };

                    ret = currency;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка получения валюты по индексу: {ex}");
            }
            return ret;
        }

        public CurrencyUi Read(string currencyName)
        {
            CurrencyUi ret = null;
            try
            {
                var currencyFromStorage = CurrencyRepository.ReadAll().FirstOrDefault(p=>p.CurrencyCode.Equals(currencyName,StringComparison.OrdinalIgnoreCase));
                if (currencyFromStorage != null)
                {
                    var currency = new CurrencyUi()
                    {
                        CurrencyId = currencyFromStorage.Id,
                        CurrencyCode = currencyFromStorage.CurrencyCode
                    };

                    ret = currency;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка получения валюты по индексу: {ex}");
            }
            return ret;
        }

        public void Update(CurrencyUi item)
        {
            try
            {
                if (item != null)
                {
                    var updateCurrencyData = CurrencyRepository.Read(item.CurrencyId);
                    if (updateCurrencyData != null)
                    {
                        updateCurrencyData.CurrencyCode = item.CurrencyCode;
                        CurrencyRepository.Update(updateCurrencyData);
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка изменения валюты в хранилище данных: {ex}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                CurrencyRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка удаления валюты по индексу: {ex}");
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
