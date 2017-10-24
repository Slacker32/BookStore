using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    internal class PromocodeData
    {
        public int Id { get; set; }
        public string Code { get; set; }// значение для сравнения
        public DateTime Date { get; set; }// дата, время задания значения(учитывается последнее значение)
    }

}
