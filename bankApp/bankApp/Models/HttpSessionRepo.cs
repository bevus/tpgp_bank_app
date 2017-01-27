using System.Web;
using System.Web.SessionState;
using BankApp.Models;

namespace bankApp.Models
{
    public class HttpSessionRepo : ISessionRepo
    {
        private HttpSessionStateBase session;

        public HttpSessionRepo(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public Customer GetCustomer()
        {
            var customerRep = new EFCustomerRepo(new BankContext());
            return customerRep.GetCustomerByID(2);
        }

        public Banker GetBanker()
        {
            throw new System.NotImplementedException();
        }
    }
}