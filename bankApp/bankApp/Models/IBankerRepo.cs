using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    interface IBankerRepo
    {
        IEnumerable<Banker> GetBankers();
        Banker GetBankerByID(int bankerId);
        void InsertBanker(Banker banker);
        void DeleteBanker(int bankerId);
        void UpdateBanker(Banker banker);
        void Save();
    }
}
