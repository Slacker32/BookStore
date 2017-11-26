using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopCore.WorkWithUi
{
    internal static class ConvertEntity
    {
        internal static AuthorUi ToAuthorUi(AuthorData author)
        {
            var authorUi = new AuthorUi()
            {
                AuthorId = author.Id,
                Info = author.Info,
                Name = author.Name,
                Year = author.Year
            };
            return authorUi;
        }
        internal static List<AuthorUi> ToListAuthorUi(IList<AuthorData> listAuthor)
        {
            var ret = new List<AuthorUi>();
            foreach (var author in listAuthor)
            {
                ret.Add(ToAuthorUi(author));
            }
            return ret;
        }
        internal static AuthorData ToAuthorData(AuthorUi author)
        {
            var authorData = new AuthorData()
            {
                Id = author.AuthorId,
                Info = author.Info,
                Name = author.Name,
                Year = author.Year
            };
            return authorData;
        }
        internal static List<AuthorData> ToListAuthorData(IList<AuthorUi> listAuthor)
        {
            var ret = new List<AuthorData>();
            foreach (var author in listAuthor)
            {
                ret.Add(ToAuthorData(author));
            }
            return ret;
        }


        internal static BookNameUi ToBookNameUi(NameBooksTranslateData nameBook)
        {
            var nameBookUi = new BookNameUi()
            {
                // ид наименования
                BookNameId = nameBook.Id,
                //язык
                LanguageBookCode = ToLanguageUi(nameBook.Language),
                //название книги
                Name = nameBook.NameBook 

            };
            return nameBookUi;
        }
        internal static List<BookNameUi> ToListBookNameUi(IList<NameBooksTranslateData> listNameBook)
        {
            var ret = new List<BookNameUi>();
            ret = listNameBook.Select(ToBookNameUi).ToList();
            return ret;
        }
        internal static NameBooksTranslateData ToBookNameData(BookNameUi nameBook)
        {
            var nameBookData = new NameBooksTranslateData()
            {
                // ид наименования
                Id = nameBook.BookNameId,
                NameBook = nameBook.Name,
                Language = ToLanguageData(nameBook.LanguageBookCode),
                BookDataId = nameBook.BookNameId

            };
            return nameBookData;
        }
        internal static List<NameBooksTranslateData> ToListBookNameData(IList<BookNameUi> listNameBook)
        {
            var ret = new List<NameBooksTranslateData>();
            ret = listNameBook.Select(ToBookNameData).ToList();
            return ret;
        }


        internal static PriceUi ToPriceUi(PricePolicyData pricePolicy)
        {
            var priceUi = new PriceUi()
            {
                PriceId = pricePolicy.Id,
                Price = pricePolicy.Price,
                Currency = ToCurrencyUi(pricePolicy.Currency),
                Country = ToCountryUi(pricePolicy.Country)
            };
           
            return priceUi;
        }
        internal static List<PriceUi> ToListPriceUi(IList<PricePolicyData> listPricePolicy)
        {
            var ret = new List<PriceUi>();
            ret = listPricePolicy.Select(ToPriceUi).ToList();
            return ret;
        }
        internal static PricePolicyData ToPricePolicyData(PriceUi price)
        {
            var priceData = new PricePolicyData()
            {
                Id = price.PriceId,
                Price = price.Price,
                Currency = ToCurrencyData(price.Currency),
                Country = ToCountryData(price.Country)
            };

            return priceData;
        }
        internal static List<PricePolicyData> ToListPricePolicyData(IList<PriceUi> listPrice)
        {
            var ret = new List<PricePolicyData>();
            ret = listPrice.Select(ToPricePolicyData).ToList();
            return ret;
        }

        internal static CurrencyUi ToCurrencyUi(CurrencyData currency)
        {
            var currencyUi = new CurrencyUi()
            {
                CurrencyId = currency.Id,
                CurrencyCode = currency.CurrencyCode,
                CurrencyName = currency.CurrencyName
            };
            return currencyUi;
        }
        internal static List<CurrencyUi> ToListCurrencyUi(IList<CurrencyData> listCurrency)
        {
            var ret = new List<CurrencyUi>();
            foreach (var item in listCurrency)
            {
                ret.Add(ToCurrencyUi(item));
            }
            return ret;
        }
        internal static CurrencyData ToCurrencyData(CurrencyUi currency)
        {
            var currencyData = new CurrencyData()
            {
                Id = currency.CurrencyId,
                CurrencyCode = currency.CurrencyCode,
                CurrencyName = currency.CurrencyName
            };
            return currencyData;
        }
        internal static List<CurrencyData> ToListCurrencyData(IList<CurrencyUi> listCurrency)
        {
            var ret = new List<CurrencyData>();
            foreach (var item in listCurrency)
            {
                ret.Add(ToCurrencyData(item));
            }
            return ret;
        }

        internal static LanguageUi ToLanguageUi(LanguageData language)
        {
            var languageUi = new LanguageUi()
            {
                LanguageId = language.Id,
                LanguageCode = language.LanguageCode,
                LanguageName = language.LanguageName
            };
            return languageUi;
        }
        internal static List<LanguageUi> ToListLanguageUi(IList<LanguageData> listLanguage)
        {
            var ret = new List<LanguageUi>();
            foreach (var item in listLanguage)
            {
                ret.Add(ToLanguageUi(item));
            }
            return ret;
        }
        internal static LanguageData ToLanguageData(LanguageUi language)
        {
            var languageData = new LanguageData()
            {
                Id = language.LanguageId,
                LanguageCode = language.LanguageCode,
                LanguageName = language.LanguageName
            };
            return languageData;
        }
        internal static List<LanguageData> ToListLanguageData(IList<LanguageUi> listLanguage)
        {
            var ret = new List<LanguageData>();
            foreach (var item in listLanguage)
            {
                ret.Add(ToLanguageData(item));
            }
            return ret;
        }

        internal static CountryUi ToCountryUi(CountryData country)
        {
            var countryUi = new CountryUi()
            {
                CountryId = country.Id,
                CountryCode = country.CountryCode,
                CountryName = country.CountryName
            };
            return countryUi;
        }
        internal static List<CountryUi> ToListCountryUi(IList<CountryData> listCountry)
        {
            var ret = new List<CountryUi>();
            foreach (var item in listCountry)
            {
                ret.Add(ToCountryUi(item));
            }
            return ret;
        }
        internal static CountryData ToCountryData(CountryUi country)
        {
            var countryData = new CountryData()
            {
                Id = country.CountryId,
                CountryCode = country.CountryCode,
                CountryName = country.CountryName
            };
            return countryData;
        }
        internal static List<CountryData> ToListCountryData(IList<CountryUi> listCountry)
        {
            var ret = new List<CountryData>();
            foreach (var item in listCountry)
            {
                ret.Add(ToCountryData(item));
            }
            return ret;
        }

        internal static FormatBookUi ToFormatBookUi(FormatBookData formatBook)
        {
            var formatBookUi = new FormatBookUi()
            {
                FormatBookId = formatBook.Id,
                FormatName = formatBook.FormatName
            };
            return formatBookUi;
        }
        internal static List<FormatBookUi> ToListFormatBookUi(IList<FormatBookData> listFormatBook)
        {
            var ret = new List<FormatBookUi>();
            foreach (var item in listFormatBook)
            {
                ret.Add(ToFormatBookUi(item));
            }
            return ret;
        }
        internal static FormatBookData ToFormatBookData(FormatBookUi formatBook)
        {
            var formatBookData = new FormatBookData()
            {
                 Id=formatBook.FormatBookId,
                 FormatName=formatBook.FormatName
            };
            return formatBookData;
        }
        internal static List<FormatBookData> ToListFormatBookData(IList<FormatBookUi> listFormatBook)
        {
            var ret = new List<FormatBookData>();
            foreach (var item in listFormatBook)
            {
                ret.Add(ToFormatBookData(item));
            }
            return ret;
        }

    }
}
