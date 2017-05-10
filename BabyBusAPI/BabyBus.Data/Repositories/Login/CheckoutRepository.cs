using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Data.Infrastructure;
using BabyBus.Model.Entities.Login;

namespace BabyBus.Data.Repositories.Login
{
    public class CheckoutRepository : RepositoryBase<Checkout>,ICheckoutRepository
    {
        public CheckoutRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) {
        }
    }

    public interface ICheckoutRepository : IRepository<Checkout>
    {
        
    }
}
