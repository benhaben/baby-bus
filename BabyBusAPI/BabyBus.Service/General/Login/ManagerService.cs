using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Model.Entities.Login;

namespace BabyBus.Service.General
{
//    public interface IManagerService
//    {
//        Manager GetManager(int managerID);
//        IEnumerable<Manager> GetAllManager();
//        void CreateManager(Manager manager);
//        void DeleteManager(int id);
//        void EditManager(Manager manager);
//        void SaveManager();
//    }
//    public class ManagerService : IManagerService
//    {
//        private readonly IManagerRepository managerRepository;
//        private readonly IUnitOfWork unitOfWork;
//
//        public ManagerService(IManagerRepository managerRepository, IUnitOfWork unitOfWork)
//        {
//            this.managerRepository = managerRepository;
//            this.unitOfWork = unitOfWork;
//        }
//
//        public Manager GetManager(int managerID)
//        {
//            return managerRepository.Get(u => u.ManagerId == managerID);
//        }
//
//        public IEnumerable<Manager> GetAllManager()
//        {
//            var manager = managerRepository.GetAll();
//            return manager;
//        }
//
//
//        public void CreateManager(Manager manager)
//        {
//            managerRepository.Add(manager);
//            SaveManager();
//        }
//        public void DeleteManager(int id)
//        {
//            var manager = managerRepository.GetById(id);
//            managerRepository.Delete(manager);
//            SaveManager();
//        }
//        public void EditManager(Manager manager)
//        {
//            managerRepository.Update(manager);
//            SaveManager();
//        }
//        public void SaveManager()
//        {
//            unitOfWork.Commit();
//        }
//
//    }
}
