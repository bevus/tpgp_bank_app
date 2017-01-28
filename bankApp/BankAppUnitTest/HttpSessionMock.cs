using System.Collections.Generic;
using System.Web;
using MvcContrib.TestHelper.Fakes;

namespace BankAppUnitTest
{
    public class HttpSessionMock : HttpSessionStateBase
    {
        private Dictionary<string, object> session = new Dictionary<string, object>();

        public override object this[string name]
        {
            get { return session[name]; }
            set { session[name] = value; }
        }
    }
}