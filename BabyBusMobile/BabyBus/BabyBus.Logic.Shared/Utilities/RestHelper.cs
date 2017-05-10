using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;

using System.Diagnostics;
using System.Net;

namespace BabyBus.Logic.Shared
{
   
    public class RestHelper
    {
        private HttpClient _baseClient;

        private HttpClient BaseClient
        {
            get { return _baseClient ?? (_baseClient = new HttpClient{ BaseAddress = new Uri(Constants.BaseUrl) }); }
        }
        //通过用户名和密码推送一个连接

        public Task<string> GetJsonString(string url)
        {
            #if DEBUG1
            Task.Delay(3000).Wait();
            #endif
            Task<string> ret = new Task<string>(() => "");
            try
            {
                var res = BaseClient.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }
        //获取一个json路径的字符串

        //		public Task<T> GetAsync<T>(string url) where T : ServiceResource
        //		{
        //			return GetAsync(typeof(T), url).ContinueWith<T>(t => (T)t.Result);
        //		}
        //		private async Task<Object> GetAsync(Type outputType, string url)
        //		{
        //			using (var client = Session.Current.ServiceClient.Create())
        //			{
        //				return await client.GetAsync(outputType, url);
        //			}
        //		}

        /// <summary>
        /// Query方法
        /// </summary>
        /// <typeparam name="T">query对象类型</typeparam>
        /// <param name="url">odata方式的url</param>
        /// <returns></returns>
        public async Task<ApiResult<T>> AsyncQuery<T>(string url)
        {
            try
            {
                var res = BaseClient.GetAsync(url).Result;
                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json))
                {
                    return new ApiResult<T>
                    {
                        Status = false,
                        Message = "服务器没有返回。"
                    };
                }
                var result = JsonConvert.DeserializeObject<ApiResult<T>>(json);
                if (result == null)
                {
                    return new ApiResult<T>
                    {
                        Status = false,
                        Message = "服务器返回的数据格式错误。"
                    };
                }

                return result;
            }
            catch (Exception e)
            {
                return new ApiResult<T>
                {
                    Status = false,
                    Message = e.Message
                };
            }
        }

       
        /// <summary>
        /// Update的Get方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ApiResponser> AsyncUpdate(string url)
        {
            try
            {
                var res = BaseClient.GetAsync(url).Result;
                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json))
                {
                    return new ApiResponser
                    {
                        Status = false,
                        Message = "服务器没有返回。"
                    };
                }
                var result = JsonConvert.DeserializeObject<ApiResponser>(json);
                if (result == null)
                {
                    return new ApiResponser
                    {
                        Status = false,
                        Message = "服务器返回的数据格式错误。"
                    };
                }

                return result;

                
            }
            catch (Exception e)
            {
                return new ApiResponser
                {
                    Status = false,
                    Message = e.Message
                };
            }
        }

        /// <summary>
        /// Update的Post方法
        /// </summary>
        /// <typeparam name="T">Attach对象类型</typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post对象</param>
        /// <returns></returns>
        public async Task<ApiResponser> AsyncUpdate(string url, object postDataParam)
        {
            ApiResponser result;

            Debug.WriteLine("send to service - url : " + url);
            try
            {
                var postdata = JsonConvert.SerializeObject(postDataParam);

                Debug.WriteLine("send to service - data : " + postdata);

                var content = new StringContent(postdata);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                var response = await BaseClient.PostAsync(url, content);
                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json))
                {
                    return new ApiResponser
                    {
                        Status = false,
                        Message = "服务器没有返回。"
                    };
                }
                result = JsonConvert.DeserializeObject<ApiResponser>(json);
                if (result == null)
                {
                    return new ApiResponser
                    {
                        Status = false,
                        Message = "服务器返回的数据格式错误。"
                    };
                }
            }
            catch (WebException e)
            {
                var errStr = string.Empty;

                switch (e.Status)
                {
                    case WebExceptionStatus.NameResolutionFailure:
                        errStr = Constants.WebEx_NameResolutionFailure;
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        errStr = Constants.WebEx_ConnectFailure;
                        break;
                }

                return new ApiResponser
                {
                    Status = false,
                    Message = errStr
                };
            }
            catch (Exception e)
            {
                return new ApiResponser
                {
                    Status = false,
                    Message = Constants.WebEx_Default
                };
            }
            return result;
        }

        public async Task<ApiResponser> ImageUpload(Byte[] bytes)
        {
            try
            {
                var temp = Convert.ToBase64String(bytes);

                var url = Constants.BaseUrl + "/Image";
                var content = new StringContent(temp);
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                var response = BaseClient.PostAsync(url, content).Result;
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResponser>(json);
                return result;
            }
            catch (Exception e)
            {
                return new ApiResponser
                {
                    Status = false,
                    Message = e.Message
                };
            }
        }
    }
}
