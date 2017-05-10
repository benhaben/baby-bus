using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Main;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Service.General.Main {
    public interface IChildService {
        Child GetChild(int childId);
        IQueryable<Child> GetAllChild();

        void CreateChild(Child child);
        void DeleteChild(int id);
        void EditChild(Child child);
        void SaveChild();
    }

    public class ChildService : IChildService {
        private readonly IChildRepository childRepository;
        private readonly IUnitOfWork unitOfWork;

        public ChildService(IChildRepository childRepository, IUnitOfWork unitOfWork) {
            this.childRepository = childRepository;
            this.unitOfWork = unitOfWork;
        }

        public Child GetChild(int childId) {
            return childRepository.Get(u => u.ChildId == childId);
        }

        public IQueryable<Child> GetAllChild() {
            IQueryable<Child> child = childRepository.GetAll();
            return child;
        }


        public void CreateChild(Child child) {
            childRepository.Add(child);
            SaveChild();
        }

        public void DeleteChild(int id) {
            Child child = childRepository.GetById(id);
            childRepository.Delete(child);
            SaveChild();
        }

        public void EditChild(Child child) {
            childRepository.Update(child);
            SaveChild();
        }

        public void SaveChild() {
            unitOfWork.Commit();
        }
    }
}