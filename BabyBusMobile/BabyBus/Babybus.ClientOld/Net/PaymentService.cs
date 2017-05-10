using System;
using System.Threading.Tasks;

using BabyBus.Logic.Shared;
using Newtonsoft.Json;



namespace BabyBus.Logic.Shared
{
    public class PaymentService:IPaymentService
    {
        private RestHelper restHelper = new RestHelper();

        public async Task<string> GenarateNewOrderNumber(PayOrder order)
        {
            var url = Constants.GenaratePayOrderNumber;
            var result = await restHelper.AsyncUpdate(url, order);
            if (result != null)
            {
                order = JsonConvert.DeserializeObject<PayOrder>(result.Attach.ToString());
                if (order != null)
                {
                    return order.OrderNumber;
                }
            }
            return string.Empty;
        }

        public async Task<int> GetPaymentFee(PaymentType type)
        {
            string url = string.Format(Constants.GetPaymentFee, (int)type);
            ApiResponser response;
            try
            {
                response = await restHelper.AsyncUpdate(url);
                if (response != null && response.Attach != null)
                {
                    return Convert.ToInt32(response.Attach);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<PayOrder> GetPayOrder(string orderNo)
        {
            string url = string.Format(Constants.GetPayOrderByOrderNo, orderNo);
            ApiResult<PayOrder> result;
            try
            {
                string json = await restHelper.GetJsonString(url);
                result = JsonConvert.DeserializeObject<ApiResult<PayOrder>>(json);
                if (result == null || result.Items.Count == 0)
                {
                    return null;
                }
                return result.Items[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

