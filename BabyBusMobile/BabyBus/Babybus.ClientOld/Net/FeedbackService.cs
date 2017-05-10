using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using BabyBus.Logic.Shared;
using Newtonsoft.Json;



namespace BabyBus.Logic.Shared
{
	
    public class FeedbackService : IFeedbackService
    {
        private RestHelper restHeler = new RestHelper();

        public async Task<ApiResponser> SendFeedback(FeedbackModel model)
        {
            var response = await restHeler.AsyncUpdate(Constants.SendFeedback, model);
            return response;
        }
    }
}
