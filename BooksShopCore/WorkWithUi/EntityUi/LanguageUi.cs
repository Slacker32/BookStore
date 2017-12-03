using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.EntityUi
{
    public class LanguageUi // тип данных язык
    {
        public int LanguageId { get; set; }// уникальный идентификатор
        public string LanguageCode { get; set; }// строковый код языка по ISO
        public string LanguageName { get; set; }// название языка

        public override string ToString()
        {
            return $"Id={LanguageId};LanguageCode={LanguageCode};LanguageName={LanguageName}";
        }
    }
}
