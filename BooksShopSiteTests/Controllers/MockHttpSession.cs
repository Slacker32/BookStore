using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BooksShopSiteTests.Controllers
{
    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> sessionDictionary = new Dictionary<string, object>();
        public override object this[string name]
        {
            get { return sessionDictionary[name]; }
            set { sessionDictionary[name] = value; }
        }
        public MockHttpSession(string name, object val)
        {
            this.sessionDictionary.Add(name, val);
        }
        public MockHttpSession(Dictionary<string, object> dict)
        {
            this.sessionDictionary = dict;
        }
    }

}
