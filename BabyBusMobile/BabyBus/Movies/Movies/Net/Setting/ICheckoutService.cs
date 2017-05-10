using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Services;

namespace BabyBus.Net.Setting
{
    public interface ICheckoutService
    {
        Task<ApiResult<CheckoutModel>> GetCheckoutList(User user);

        Task<ApiResponser> Checkout(CheckoutModel check);
    }
}
