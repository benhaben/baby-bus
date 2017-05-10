using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;

namespace AdministratorManagement.Controllers
{
     [System.Web.Http.Authorize]
    public class AnalysisController : BabyBusApiController
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AnalysisController));

        public enum EAnalysisType
        {
            AnalysisParentPerDay = 0,
            AnalysisKindergartenPerDay = 1,
        }

        public class AnalysisParameter
        {
            public EAnalysisType AnalysisType { get; set; }
            public DateTimeOffset BeginDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
        }

        public ApiResponser Post([FromBody]AnalysisParameter value)
        {
            StartWatch();
            var response = new ApiResponser(true);
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    if (value.AnalysisType == EAnalysisType.AnalysisParentPerDay)
                    {
                        db.sp_updateAnalysisParentTable();
                        var data = from i in db.AnalysisParent
                            where i.Date >= value.BeginDate && i.Date <= value.EndDate
                            select i;
                        response.Attach = data.ToList();
                    }
                    else if (value.AnalysisType == EAnalysisType.AnalysisKindergartenPerDay)
                    {
                        db.sp_updateAnalysisKindergartenTable();
                        var data = from i in db.AnalysisKindergarten
                            where i.Date >= value.BeginDate && i.Date <= value.EndDate
                            select i;
                        response.Attach = data.ToList();
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "AnalysisType 指定错误";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                _log.Fatal(ex.Message, ex);
                return response;
            }
            EndWatch();
            return response;
        }

    }
}