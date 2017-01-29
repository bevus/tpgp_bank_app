namespace bankApp.Util
{
    public interface ICredit
    {
        bool CheckCredit(int requestedAmount, int householdIncomes, int contribution, int duration);
    }
}