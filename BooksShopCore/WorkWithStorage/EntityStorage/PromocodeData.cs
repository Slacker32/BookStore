using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("Promocodes")]
    internal class PromocodeData
    {
        public int Id { get; set; }
        public string Code { get; set; }// значение для сравнения
        public int Percent { get; set; }// процент скидки для промокода
        public DateTime Date { get; set; }// дата, время задания значения(учитывается последнее значение)
    }

}
