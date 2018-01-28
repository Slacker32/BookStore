using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.WorkWithDataStorage
{
    public class WorkWithCountryStorage : IDisposable, IWorkWithDataStorage<CountryUi>
    {
        private IDataRepository<CountryData> CountryRepository { get; set; }
        private IDataRepositoryAsync<CountryData> CountryRepositoryAsync { get; set; }

        public WorkWithCountryStorage()
        {
            CountryRepository = new GenericRepository<CountryData>(new BookStoreContext());
            CountryRepositoryAsync = new GenericRepositoryAsync<CountryData,BookStoreContext>();

        }

        public IList<CountryUi> ReadAll()
        {
            IList<CountryUi> ret = null;
            try
            {
                var countryFromStorage = CountryRepository.ReadAll();
                if (countryFromStorage != null)
                {
                    foreach (var item in countryFromStorage)
                    {
                        var country = new CountryUi()
                        {
                            CountryId = item.Id,
                            CountryCode = item.CountryCode,
                            CountryName = item.CountryName
                        };

                        if (ret == null)
                        {
                            ret = new List<CountryUi>();
                        }
                        ret.Add(country);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения данных по всем странам из хранилища");
                throw;
            }

            return ret;
        }
        public async Task<IList<CountryUi>> ReadAllAsync()
        {
            IList<CountryUi> ret = null;
            try
            {
                var countryFromStorage = await CountryRepositoryAsync.ReadAllAsync();
                if (countryFromStorage != null)
                {
                    foreach (var item in countryFromStorage)
                    {
                        var country = new CountryUi()
                        {
                            CountryId = item.Id,
                            CountryCode = item.CountryCode,
                            CountryName = item.CountryName
                        };

                        if (ret == null)
                        {
                            ret = new List<CountryUi>();
                        }
                        ret.Add(country);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения данных по всем странам из хранилища");
                throw;
            }

            return ret;
        }

        public void Create(CountryUi item)
        {
            try
            {
                if (item != null)
                {
                    var countryData = new CountryData()
                    {
                        CountryCode = item.CountryCode,
                        CountryName = item.CountryName
                    };

                    CountryRepository.Create(countryData);
                    CountryRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления страны в хранилище данных");
                throw;
            }
        }
        public async Task CreateAsync(CountryUi item)
        {
            try
            {
                if (item != null)
                {
                    var countryData = new CountryData()
                    {
                        CountryCode = item.CountryCode,
                        CountryName = item.CountryName
                    };

                    await CountryRepositoryAsync.CreateAsync(countryData);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления страны в хранилище данных");
                throw;
            }
        }

        public CountryUi Read(int id)
        {
            CountryUi ret = null;
            try
            {
                var countryFromStorage = CountryRepository.Read(id);
                if (countryFromStorage != null)
                {
                    var country = new CountryUi()
                    {
                        CountryId = countryFromStorage.Id,
                        CountryCode = countryFromStorage.CountryCode,
                        CountryName = countryFromStorage.CountryName
                    };

                    ret = country;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения страны из хранилища по индексу");
                throw;
            }
            return ret;
        }
        public async Task<CountryUi> ReadAsync(int id)
        {
            CountryUi ret = null;
            try
            {
                var countryFromStorage = await CountryRepositoryAsync.ReadAsync(id);
                if (countryFromStorage != null)
                {
                    var country = new CountryUi()
                    {
                        CountryId = countryFromStorage.Id,
                        CountryCode = countryFromStorage.CountryCode,
                        CountryName = countryFromStorage.CountryName
                    };

                    ret = country;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения страны из хранилища по индексу");
                throw;
            }
            return ret;
        }

        public CountryUi Read(string countryCode)
        {
            CountryUi ret = null;
            try
            {
                var countryFromStorage = CountryRepository.ReadAll().FirstOrDefault(p => p.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
                if (countryFromStorage != null)
                {
                    var country = new CountryUi()
                    {
                        CountryId = countryFromStorage.Id,
                        CountryCode = countryFromStorage.CountryCode,
                        CountryName = countryFromStorage.CountryName
                    };

                    ret = country;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения страны из хранилища по коду страны");
                throw;
            }
            return ret;
        }
        public async Task<CountryUi> ReadAsync(string countryCode)
        {
            CountryUi ret = null;
            try
            {
                var countryFromStorageList = await CountryRepositoryAsync.ReadAllAsync();
                var countryFromStorage= countryFromStorageList.FirstOrDefault(p => p.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
                if (countryFromStorage != null)
                {
                    var country = new CountryUi()
                    {
                        CountryId = countryFromStorage.Id,
                        CountryCode = countryFromStorage.CountryCode,
                        CountryName = countryFromStorage.CountryName
                    };

                    ret = country;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения страны из хранилища по коду страны");
                throw;
            }
            return ret;
        }

        public void Update(CountryUi item)
        {
            try
            {
                if (item != null)
                {
                    var updateCountryData = CountryRepository.Read(item.CountryId);
                    if (updateCountryData != null)
                    {
                        updateCountryData.CountryCode = item.CountryCode;
                        updateCountryData.CountryName = item.CountryName;
                        CountryRepository.Update(updateCountryData);
                        CountryRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка обновления данных страны в хранилище");
                throw;
            }
        }
        public async Task UpdateAsync(CountryUi item)
        {
            try
            {
                if (item != null)
                {
                    var updateCountryData = await CountryRepositoryAsync.ReadAsync(item.CountryId);
                    if (updateCountryData != null)
                    {
                        updateCountryData.CountryCode = item.CountryCode;
                        updateCountryData.CountryName = item.CountryName;
                        await CountryRepositoryAsync.AddOrUpdateAsync(updateCountryData);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка обновления данных страны в хранилище");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                CountryRepository.Delete(id);
                CountryRepository.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка удаления страны из хранилища по индексу: {ex}");
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                await CountryRepositoryAsync.DeleteAsync(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка удаления страны из хранилища по индексу: {ex}");
            }
        }

        public void AddOrUpdate(CountryUi item)
        {
            try
            {
                if (item != null)
                {
                    var countryData = new CountryData()
                    {
                        CountryCode = item.CountryCode,
                        CountryName = item.CountryName
                    };

                    CountryRepository.AddOrUpdate(countryData);
                    CountryRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или обновления страны в хранилище данных");
                throw;
            }
        }
        public async Task AddOrUpdateAsync(CountryUi item)
        {
            try
            {
                if (item != null)
                {
                    var countryData = new CountryData()
                    {
                        CountryCode = item.CountryCode,
                        CountryName = item.CountryName
                    };

                    await CountryRepositoryAsync.AddOrUpdateAsync(countryData);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления или обновления страны в хранилище данных");
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
                    CountryRepository.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
