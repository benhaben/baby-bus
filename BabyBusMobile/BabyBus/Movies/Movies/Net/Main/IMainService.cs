using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.GroupMemory;
using BabyBus.Models.Main;

namespace BabyBus.Services.Main
{
    public interface IMainService
    {
//        Task<ChildInfo> GetChildById(int childId);

//        Task<ChildAllInfo> GetChildAllInfoByParentId(int userId);

//        Task<List<Article>> GetNoticeList(int userId);

        Task<bool> Logout(int userId);

        Task<ClassInfo> GetClassByTeacherId(int userId);

        Task<ApiResponser> ChangePassword(object pwd);

        Task<ApiResult<VersionModel>> GetVersions(int appType);

        Task<VersionModel> GetVersionByName(int appType);

        Task<ChildSummaryInfo> GetChildSummary(int childId);
    }
}
