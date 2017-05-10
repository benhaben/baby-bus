using System;
using System.Net.Http;

namespace AdministratorManagement.Core.Handlers
{
    public class ResponseHeaderHandler : DelegatingHandler, IDelegatingHandler
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ResponseHeaderHandler));

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    HttpResponseMessage response = null;
                    try
                    {
                        response = task.Result;
                        response.Headers.Add("X-Custom-Header", Guid.NewGuid().ToString());
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(" ResponseHeaderHandler SendAsync " + ex.Message);
                    }
                    return response;

                });
        }
    }
}