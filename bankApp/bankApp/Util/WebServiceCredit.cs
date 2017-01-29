namespace bankApp.Util
{
    public class WebServiceCredit : ICredit
    {
        private CheckCreditServiceReference.Service1Client _client;
        public WebServiceCredit()
        {
            _client = new CheckCreditServiceReference.Service1Client();
        }

        public bool CheckCredit(int requestedAmount, int householdIncomes, int contribution, int duration)
        {
            return _client.CheckCredit(requestedAmount, householdIncomes, contribution, duration);
        }
    }
}