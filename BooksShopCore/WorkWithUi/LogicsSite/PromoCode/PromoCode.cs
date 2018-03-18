using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;

namespace BooksShopCore.WorkWithUi.LogicsSite.PromoCode
{
    public class PromoCode : IPromoCode
    {
        private IDataRepository<PromocodeData> promoCodeRepository;
        private IDataRepositoryAsync<PromocodeData> promoCodeRepositoryAsync;

        public PromoCode()
        {
            promoCodeRepository = new GenericRepository<PromocodeData>(new BookStoreContext());
            promoCodeRepositoryAsync = new GenericRepositoryAsync<PromocodeData, BookStoreContext>();
        }

        public (bool,decimal) ConsiderPromoCode(string valueCode, decimal amount)
        {
            var applyPromoCode = false;
            var rezAmount = amount;
            try
            {
                var allCodeList = promoCodeRepository.ReadAll();
                var promoCodeList = allCodeList.Where(p => p.Code.Equals(valueCode)).OrderByDescending(p => p.Date).ToList();
                if (promoCodeList?.Count > 0)
                {
                    var code = promoCodeList.First();
                    if (code != null)
                    {
                        rezAmount = amount * (code.Percent / 100m);
                        applyPromoCode = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetPreview", $"Ошибка получения превью из хранилища данных: {ex.StackTrace}");
                throw;
            }
            return (applyPromoCode, rezAmount);
        }

        public async Task<(bool,decimal)> ConsiderPromoCodeAsync(string valueCode, decimal amount)
        {
            var applyPromoCode = false;
            var rezAmount = amount;
            try
            {
                var allCodeList = await promoCodeRepositoryAsync.ReadAllAsync();
                var promoCodeList = allCodeList.Where(p => p.Code.Equals(valueCode)).OrderByDescending(p=>p.Date).ToList();
                if (promoCodeList?.Count > 0)
                {
                    var code = promoCodeList.First();
                    if (code!=null)
                    {
                        rezAmount = amount * (code.Percent / 100m);
                        applyPromoCode = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetPreview", $"Ошибка получения превью из хранилища данных: {ex.StackTrace}");
                throw;
            }
            return (applyPromoCode,rezAmount);
        }


    }
}
