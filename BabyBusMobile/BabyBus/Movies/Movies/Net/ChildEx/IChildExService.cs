using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Services.ChildEx
{
    public interface IChildExService
    {
        Task<List<Models.ChildEx.ChildEx>> SearchChildEx();
        Task<Models.ChildEx.ChildEx> GetById(int id);
    }
}
