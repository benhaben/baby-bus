using System;
using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;

namespace BabyBus.Service.General
{
    public interface IAdminService
    {
        Admin GetAdmin(int AdminId);
        IQueryable<Admin> GerAllAdmins();
        void CreateAdmin(Admin admin);
        void EditAdmin(Admin admin);
        void DeleteAdmin(int id);
        void SaveAdmin();

        LoginCheckStatus CheckLoginAdmin(Admin loginAdmin);
        Admin GetAdminByLoginName(String loginName);
    }

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository adminRepository;
        private readonly IUnitOfWork unitOfWork;

        public AdminService(IAdminRepository adminRepository, IUnitOfWork unitOfWork)
        {
            this.adminRepository = adminRepository;
            this.unitOfWork = unitOfWork;
        }

        public Admin GetAdmin(int AdminId)
        {
            return adminRepository.Get(u => u.AdminId == AdminId);
        }

        public IQueryable<Admin> GerAllAdmins()
        {
            var admin = adminRepository.GetAll();
            return admin;
        }

        public void CreateAdmin(Admin admin)
        {
            adminRepository.Add(admin);
            SaveAdmin();
        }

        public void EditAdmin(Admin admin)
        {
            adminRepository.Update(admin);
            SaveAdmin();
        }

        public void DeleteAdmin(int id)
        {
            var admin = adminRepository.GetById(id);
            adminRepository.Delete(admin);
            SaveAdmin();
        }

        public void SaveAdmin()
        {
            unitOfWork.Commit();
        }

        public LoginCheckStatus CheckLoginAdmin(Admin loginAdmin)
        {
            if (adminRepository.Get(u => u.AdminLoginName == loginAdmin.AdminLoginName && u.AdminLoginPassword == loginAdmin.AdminLoginPassword) == null)
                return LoginCheckStatus.UnCorrect;
            return LoginCheckStatus.Correct;
        }

        public Admin GetAdminByLoginName(string loginName)
        {
            return adminRepository.Get(u => u.AdminLoginName == loginName);
        }
    }
}