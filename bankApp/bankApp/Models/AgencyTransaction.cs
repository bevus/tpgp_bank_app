using System;

namespace BankApp.Models
{
    public class AgencyTransaction: Transaction
    {
        public override string Label
        {
            get
            {
                var label = "";
                switch (TransactionType)
                {
                    case TransactionType.DEBIT:
                        label = "Retrait";
                        break;
                    case TransactionType.CREDIT:
                        label = "Depot";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return label + " : " + Agency;
            }
        }

        public string Agency { get; set; }
    }
}
