using System.Collections.Generic;
using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Main;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Service.General.Main {
    public interface IClassService {
        Class GetClass(int classId);
        IEnumerable<Class> GetAllClass();

        void CreateClass(Class classes);
        void DeleteClass(int id);
        void EditClass(Class classes);
        void SaveClass();
    }

    public class ClassService : IClassService {
        private readonly IClassRepository classRepository;
        private readonly IUnitOfWork unitOfWork;

        public ClassService(IClassRepository classRepository, IUnitOfWork unitOfWork) {
            this.classRepository = classRepository;
            this.unitOfWork = unitOfWork;
        }

        public Class GetClass(int classId) {
            return classRepository.Get(u => u.ClassId == classId);
        }

        public IEnumerable<Class> GetAllClass() {
            IQueryable<Class> classes = classRepository.GetAll();
            return classes;
        }

        public void CreateClass(Class classes) {
            classRepository.Add(classes);
            SaveClass();
        }

        public void DeleteClass(int id) {
            Class classes = classRepository.GetById(id);
            classRepository.Delete(classes);
            SaveClass();
        }

        public void EditClass(Class classes) {
            classRepository.Update(classes);
            SaveClass();
        }

        public void SaveClass() {
            unitOfWork.Commit();
        }
    }
}