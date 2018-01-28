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

namespace BooksShopCore.WorkWithUi.WorkWithDataStorage
{
    public class WorkWithCurrencyStorage : IDisposable, IWorkWithDataStorage<CurrencyUi>
    {
        private IDataRepository<CurrencyData> CurrencyRepository { get; set; }
        private IDataRepositoryAsync<CurrencyData> CurrencyRepositoryAsync { get; set; }

        public WorkWithCurrencyStorage()
        {
            CurrencyRepository = new GenericRepository<CurrencyData>(new BookStoreContext());
            CurrencyRepositoryAsync = new GenericRepositoryAsync<CurrencyData,BookStoreContext>();

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
                            CurrencyCode = item.CurrencyCode,
                            CurrencyName = item.CurrencyName
                        };

                        if (ret == null)
                        {
                            ret = new List<CurrencyUi>();
                        }
                        ret.Add(currency);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения всех валют из хранилища");
                throw;
            }

            return ret;
        }
        public async Task<IList<CurrencyUi>> ReadAllAsync()
        {
            IList<CurrencyUi> ret = null;
            try
            {
                var currencyFromStorage = await CurrencyRepositoryAsync.ReadAllAsync();
                if (currencyFromStorage != null)
                {
                    foreach (var item in currencyFromStorage)
                    {
                        var currency = new CurrencyUi()
                        {
                            CurrencyId = item.Id,
                            CurrencyCode = item.CurrencyCode,
                            CurrencyName = item.CurrencyName
                        };

                        if (ret == null)
                        {
                            ret = new List<CurrencyUi>();
                        }
                        ret.Add(currency);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения всех валют из хранилища");
                throw;
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
                        CurrencyCode = item.CurrencyCode,
                        CurrencyName = item.CurrencyName
                    };

                    CurrencyRepository.Create(currencyData);
                    CurrencyRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления валюты в хранилище");
                throw;
            }
        }
        public async Task CreateAsync(CurrencyUi item)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в валюты в хранилище данных
                    var currencyData = new CurrencyData()
                    {
                        CurrencyCode = item.CurrencyCode,
                        CurrencyName = item.CurrencyName
                    };

                    await CurrencyRepositoryAsync.CreateAsync(currencyData);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления валюты в хранилище");
                throw;
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
                        CurrencyCode = currencyFromStorage.CurrencyCode,
                        CurrencyName = currencyFromStorage.CurrencyName
                    };

                    ret = currency;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения валюты из хранилища по индексу");
                throw;
            }
            return ret;
        }
        public async Task<CurrencyUi> ReadAsync(int id)
        {
            CurrencyUi ret = null;
            try
            {
                var currencyFromStorage = await CurrencyRepositoryAsync.ReadAsync(id);
                if (currencyFromStorage != null)
                {
                    var currency = new CurrencyUi()
                    {
                        CurrencyId = currencyFromStorage.Id,
                        CurrencyCode = currencyFromStorage.CurrencyCode,
                        CurrencyName = currencyFromStorage.CurrencyName
                    };

                    ret = currency;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения валюты из хранилища по индексу");
                throw;
            }
            return ret;
        }

        public CurrencyUi Read(string сurrencyCode)
        {
            CurrencyUi ret = null;
            try
            {
                var currencyFromStorage = CurrencyRepository.ReadAll().FirstOrDefault(p=>p.CurrencyCode.Equals(сurrencyCode, StringComparison.OrdinalIgnoreCase));
                if (currencyFromStorage != null)
                {
                    var currency = new CurrencyUi()
                    {
                        CurrencyId = currencyFromStorage.Id,
                        CurrencyCode = currencyFromStorage.CurrencyCode,
                        CurrencyName = currencyFromStorage.CurrencyName
                    };

                    ret = currency;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения данных валюты из хранилища по коду валюты");
                throw;
            }
            return ret;
        }
        public async Task<CurrencyUi> ReadAsync(string сurrencyCode)
        {
            CurrencyUi ret = null;
            try
            {
                var currencyListFromStorage = await CurrencyRepositoryAsync.ReadAllAsync();
                var currencyFromStorage= currencyListFromStorage.FirstOrDefault(p => p.CurrencyCode.Equals(сurrencyCode, StringComparison.OrdinalIgnoreCase));
                if (currencyFromStorage != null)
                {
                    var currency = new CurrencyUi()
                    {
                        CurrencyId = currencyFromStorage.Id,
                        CurrencyCode = currencyFromStorage.CurrencyCode,
                        CurrencyName = currencyFromStorage.CurrencyName
                    };

                    ret = currency;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения данных валюты из хранилища по коду валюты");
                throw;
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
                        updateCurrencyData.CurrencyName = item.CurrencyName;
                        CurrencyRepository.Update(updateCurrencyData);
                        CurrencyRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка изменения данных валюты в хранилище");
                throw;
            }
        }
        public async Task UpdateAsync(CurrencyUi item)
        {
            try
            {
                if (item != null)
                {
                    var updateCurrencyData = await CurrencyRepositoryAsync.ReadAsync(item.CurrencyId);
                    if (updateCurrencyData != null)
                    {
                        updateCurrencyData.CurrencyCode = item.CurrencyCode;
                        updateCurrencyData.CurrencyName = item.CurrencyName;
                        await CurrencyRepositoryAsync.AddOrUpdateAsync(updateCurrencyData);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка изменения данных валюты в хранилище");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                CurrencyRepository.Delete(id);
                CurrencyRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления валюты из хранилища");
                throw;
            }
        }
        public async Task Deleteasync(int id)
        {
            try
            {
                await CurrencyRepositoryAsync.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления валюты из хранилища");
                throw;
            }
        }

        public void AddOrUpdate(CurrencyUi item)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в валюты в хранилище данных
                    var currencyData = new CurrencyData()
                    {
                        CurrencyCode = item.CurrencyCode,
                        CurrencyName = item.CurrencyName
                    };

                    CurrencyRepository.AddOrUpdate(currencyData);
                    CurrencyRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или изменения валюты в хранилище");
                throw;
            }
        }
        public async Task AddOrUpdateAsync(CurrencyUi item)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в валюты в хранилище данных
                    var currencyData = new CurrencyData()
                    {
                        CurrencyCode = item.CurrencyCode,
                        CurrencyName = item.CurrencyName
                    };

                    await CurrencyRepositoryAsync.AddOrUpdateAsync(currencyData);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или изменения валюты в хранилище");
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
                    CurrencyRepository.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion

    }
}
