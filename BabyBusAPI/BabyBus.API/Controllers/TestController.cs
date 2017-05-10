using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using BabyBus.Core.Common;

namespace BabyBus.API.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get(string password)
        {
            var enc = new UTF8Encoding();
            var data = enc.GetBytes(password);
            SHA1 sha = new SHA1CryptoServiceProvider();
            password = BitConverter.ToString(sha.ComputeHash(data)).Replace("-", "");
            return Ok(password);
        }

        //public IHttpActionResult Get(string password)
        //{
        //    var test = Sha1Encrypt.Sha1EncryptPassword(password);
        //    return Ok(test);
        //}
    }
}
