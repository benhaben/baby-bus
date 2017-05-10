using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Main;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Service.General.Main {
    public interface IKindergartenService {
        Kindergarten GetKindergarten(int kindergartenId);
        IQueryable<Kindergarten> GetAllKindergarten();

        void CreateKindergarten(Kindergarten kindergarten);
        void DeleteKindergarten(int id);
        void EditKindergarten(Kindergarten kindergarten);
        void SaveKindergarten();
    }

    public class KindergartenService : IKindergartenService {
        private readonly IKindergartenRepository kindergartenRepository;
        private readonly IUnitOfWork unitOfWork;

        public KindergartenService(IKindergartenRepository kindergartenRepository, IUnitOfWork unitOfWork) {
            this.kindergartenRepository = kindergartenRepository;
            this.unitOfWork = unitOfWork;
        }

        public Kindergarten GetKindergarten(int kindergartenId) {
            return kindergartenRepository.Get(u => u.KindergartenId == kindergartenId);
        }

        public IQueryable<Kindergarten> GetAllKindergarten() {
            IQueryable<Kindergarten> kindergarten = kindergartenRepository.GetAll();
            return kindergarten;
        }


        public void CreateKindergarten(Kindergarten kindergarten) {
            kindergartenRepository.Add(kindergarten);
            SaveKindergarten();
        }

        public void DeleteKindergarten(int id) {
            Kindergarten kindergarten = kindergartenRepository.GetById(id);
            kindergartenRepository.Delete(kindergarten);
            SaveKindergarten();
        }

        public void EditKindergarten(Kindergarten kindergarten) {
            kindergartenRepository.Update(kindergarten);
            SaveKindergarten();
        }

        public void SaveKindergarten() {
            unitOfWork.Commit();
        }
    }
}