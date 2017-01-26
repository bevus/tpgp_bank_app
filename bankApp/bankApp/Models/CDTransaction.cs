using System;

namespace BankApp.Models
{
    public class CDTransaction:Transaction
    {
        public string CashDispanserName { get; set; }
        public CDType CdType { get; set; }
        public string AgencyName { get; set; }

        public override string Label
        {
            get
            {
                switch (CdType)
                {
                    case CDType.INSIDE:
                        return "Retrait : " + CashDispanserName+ " - " + AgencyName;
                    case CDType.OUTSIDE:
                        return "Retrait : " + CashDispanserName;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
