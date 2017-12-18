using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.LogicsSite.Preview
{
    public interface IPreview
    {
        string GetPreview(int bookId);
    }

}
