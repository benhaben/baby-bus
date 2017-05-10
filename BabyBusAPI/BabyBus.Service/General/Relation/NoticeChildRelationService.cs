using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Relation;

namespace BabyBus.Service.General
{
    public interface INoticeChildRelationService
    {
        NoticeChildRelation GetNoticeChildRelation(int noticeChildRelationID);
        IEnumerable<NoticeChildRelation> GetAllNoticeChildRelation();

        void CreateNoticeChildRelation(NoticeChildRelation noticeChildRelation);
        void DeleteNoticeChildRelation(int id);
        void EditNoticeChildRelation(NoticeChildRelation noticeChildRelation);
        void SaveNoticeChildRelation();
    }
    public class NoticeChildRelationService : INoticeChildRelationService
    {
        private readonly INoticeChildRelationRepository userClassRelationRepository;
        private readonly IUnitOfWork unitOfWork;

        public NoticeChildRelationService(INoticeChildRelationRepository userClassRelationRepository, IUnitOfWork unitOfWork)
        {
            this.userClassRelationRepository = userClassRelationRepository;
            this.unitOfWork = unitOfWork;
        }

        public NoticeChildRelation GetNoticeChildRelation(int noticeChildRelationID)
        {
            return userClassRelationRepository.Get(u => u.NoticeChildRelationId == noticeChildRelationID);
        }

        public IEnumerable<NoticeChildRelation> GetAllNoticeChildRelation()
        {
            var noticeChildRelation = userClassRelationRepository.GetAll();
            return noticeChildRelation;
        }


        public void CreateNoticeChildRelation(NoticeChildRelation noticeChildRelation)
        {
            userClassRelationRepository.Add(noticeChildRelation);
            SaveNoticeChildRelation();
        }
        public void DeleteNoticeChildRelation(int id)
        {
            var noticeChildRelation = userClassRelationRepository.GetById(id);
            userClassRelationRepository.Delete(noticeChildRelation);
            SaveNoticeChildRelation();
        }
        public void EditNoticeChildRelation(NoticeChildRelation noticeChildRelation)
        {
            userClassRelationRepository.Update(noticeChildRelation);
            SaveNoticeChildRelation();
        }
        public void SaveNoticeChildRelation()
        {
            unitOfWork.Commit();
        }

    }
}
