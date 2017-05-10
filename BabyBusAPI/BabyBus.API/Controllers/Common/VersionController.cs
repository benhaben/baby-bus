using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Service.Models;
using Newtonsoft.Json;

namespace BabyBus.API.Controllers.Common {
    public class VersionController : ApiController {
        private readonly string[] _apkstrings = {
            "",
            "api/v2/app/install/54d86916133d1b6b2a001a33?token=f81f8ae0ad1611e4a52a7442c625544240c49737",
            "api/v2/app/install/54d8699eca4acb6078001c4c?token=f81f8ae0ad1611e4a52a7442c625544240c49737",
            "api/v2/app/install/54d8681d4d0d245878001abc?token=f81f8ae0ad1611e4a52a7442c625544240c49737"
        };

        //Api:Get
        public ApiResult<VersionModel> Get(int appType) {
            var result = new ApiResult<VersionModel>();

            string fullName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.VersionList);

            try {
                string json;
                using (var sr = new StreamReader(fullName)) {
                    json = sr.ReadToEnd();
                }
                var versions = JsonConvert.DeserializeObject<List<VersionModel>>(json);
                string baseurl = Config.ApkBaseUrl;
                versions.ForEach(x => { x.Link = baseurl + _apkstrings[x.AppType]; });

                var returnlist = new List<VersionModel>();
                if (appType != 0) {
                    foreach (VersionModel version in versions) {
                        if (appType == version.AppType) {
                            returnlist.Add(version);
                        }
                    }
                }
                else {
                    returnlist = versions;
                }
                result.Status = true;
                result.Items = returnlist;
                result.TotalCount = returnlist.Count;
                return result;
            }
            catch (Exception ex) {
                result.Status = false;
                result.Message = ex.Message;
                result.TotalCount = 0;
                return result;
            }
        }
    }
}