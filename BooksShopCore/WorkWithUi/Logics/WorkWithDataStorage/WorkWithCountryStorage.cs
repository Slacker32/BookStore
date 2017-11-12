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
    public class WorkWithCountryStorage : IDisposable, IWorkWithDataStorage<CountryUi>
    {
        private readonly BookStoreContext db;
        private IDataRepository<CountryData> CountryRepository { get; set; }

        public WorkWithCountryStorage()
        {
            this.db = new BookStoreContext();
            CountryRepository = new GenericRepository<CountryData>(db);

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
            catch (Exception)
            {
                throw new ApplicationException($"Ошибка получения данных");
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
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка добавления страны в хранилище данных: {ex}");
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

                throw new ApplicationException($"Ошибка получения страны по индексу: {ex}");
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

                throw new ApplicationException($"Ошибка получения страны по коду: {ex}");
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
                        this.db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка изменения страны в хранилище данных: {ex}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                CountryRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"Ошибка удаления страны по индексу: {ex}");
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
