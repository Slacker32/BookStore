using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage.EntityStorage
{
    [Table("FormatsPreview")]
    internal class FormatPreviewData
    {
        public int Id { get; set; }
        public string FormatName { get; set; }// формат данных для превью(pdf, jpg, xml)
    }

}
