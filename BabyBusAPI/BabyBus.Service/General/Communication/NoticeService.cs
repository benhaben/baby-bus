using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Article;
using BabyBus.Data.Repositories.Main;
using BabyBus.Model.Entities.Article;
using BabyBus.Model.Entities.Article.Enum;

namespace BabyBus.Service.General.Communication {
    public interface INoticeService {
        Notice GetNotice(int noticeId);
        IQueryable<Notice> GetAllNotice();
        long CreateNotice(Notice notice);
        void DeleteNotice(int noticeId);
        void EditNotice(Notice notice);
        void SaveNotice();
    }

    public class NoticeService : INoticeService {
        private readonly IClassRepository _classRepository;
        private readonly IKindergartenRepository _kgRepository;
        private readonly INoticeRepository _noticeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NoticeService(INoticeRepository noticeRepository,
            IKindergartenRepository kgRepository,
            IClassRepository classRepository,
            IUnitOfWork unitOfWork) {
            _noticeRepository = noticeRepository;
            _kgRepository = kgRepository;
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public Notice GetNotice(int noticeId) {
            return _noticeRepository.Get(u => u.NoticeId == noticeId);
        }

        public IQueryable<Notice> GetAllNotice() {
            IQueryable<Notice> classNotice = _noticeRepository.GetAll();
            return classNotice;
        }


        public long CreateNotice(Notice notice) {
            if (notice.NoticeType == NoticeType.ClassHomework
                || notice.NoticeType == NoticeType.ClassCommon
                || notice.NoticeType == NoticeType.ClassEmergency) {
                notice.ReceiverNumber = _classRepository.GetById(notice.ClassId).ClassCount;
            }
            else {
                notice.ReceiverNumber = _kgRepository.GetById(notice.KindergartenId).KindergartenCount;
            }

            _noticeRepository.Add(notice);
            SaveNotice();
            return notice.NoticeId;
        }

        public void DeleteNotice(int noticeId) {
            Notice classNotice = _noticeRepository.GetById(noticeId);
            _noticeRepository.Delete(classNotice);
            SaveNotice();
        }

        public void EditNotice(Notice notice) {
            _noticeRepository.Update(notice);
            SaveNotice();
        }

        public void SaveNotice() {
            _unitOfWork.Commit();
        }
    }
}