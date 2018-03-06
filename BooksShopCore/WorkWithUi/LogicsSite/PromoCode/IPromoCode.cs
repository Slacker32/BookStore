using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithUi.LogicsSite.PromoCode
{
    public interface IPromoCode
    {
        bool ConsiderPromoCode(ref decimal amount);
    }

}
