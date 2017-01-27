using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BankApp.Models
{
    public class EFBankerRepo : IBankerRepo
    {
        private BankContext context;
        public EFBankerRepo(BankContext context)
        {
            this.context = context;
        }

        public IEnumerable<Banker> GetBankers()
        {
            return context.Bankers.Include(b => b.Customers).ToList();
        }

        public Banker GetBankerByID(int bankerId)
        {
            return context.Bankers.Include(b => b.Customers).Single(b => b.ID == bankerId);
        }

        public void InsertBanker(Banker banker)
        {
            context.Bankers.Add(banker);
        }

        public void DeleteBanker(int bankerId)
        {
            var banker = context.Bankers.Find(bankerId);
            context.Bankers.Remove(banker);
        }

        public void UpdateBanker(Banker banker)
        {
            context.Entry(banker).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}