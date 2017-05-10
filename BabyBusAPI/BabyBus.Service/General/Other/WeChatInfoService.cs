using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Other;
using BabyBus.Model.Entities.Other;

namespace BabyBus.Service.General
{
    public interface IWeChatInfoService
    {
        WeChatInfo GetWeChatInfo(int WeChatInfoID);
        IEnumerable<WeChatInfo> GetAllWeChatInfo();

        void CreateWeChatInfo(WeChatInfo WeChatInfo);
        void DeleteWeChatInfo(int id);
        void EditWeChatInfo(WeChatInfo WeChatInfo);
        void SaveWeChatInfo();
    }
    public class WeChatInfoService : IWeChatInfoService
    {
        private readonly IWeChatInfoRepository WeChatInfoRepository;
        private readonly IUnitOfWork unitOfWork;

        public WeChatInfoService(IWeChatInfoRepository WeChatInfoRepository, IUnitOfWork unitOfWork)
        {
            this.WeChatInfoRepository = WeChatInfoRepository;
            this.unitOfWork = unitOfWork;
        }

        public WeChatInfo GetWeChatInfo(int WeChatInfoID)
        {
            return WeChatInfoRepository.Get(u => u.WeChatInfoId == WeChatInfoID);
        }

        public IEnumerable<WeChatInfo> GetAllWeChatInfo()
        {
            var WeChatInfo = WeChatInfoRepository.GetAll();
            return WeChatInfo;
        }


        public void CreateWeChatInfo(WeChatInfo WeChatInfo)
        {
            WeChatInfoRepository.Add(WeChatInfo);
            SaveWeChatInfo();
        }
        public void DeleteWeChatInfo(int id)
        {
            var WeChatInfo = WeChatInfoRepository.GetById(id);
            WeChatInfoRepository.Delete(WeChatInfo);
            SaveWeChatInfo();
        }
        public void EditWeChatInfo(WeChatInfo WeChatInfo)
        {
            WeChatInfoRepository.Update(WeChatInfo);
            SaveWeChatInfo();
        }
        public void SaveWeChatInfo()
        {
            unitOfWork.Commit();
        }

    }
}
