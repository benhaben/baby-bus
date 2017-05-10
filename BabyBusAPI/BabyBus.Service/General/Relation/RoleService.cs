using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Relation;

namespace BabyBus.Service.General
{
    public interface IRoleService
    {
        Role GetRole(int roleID);
        IEnumerable<Role> GetAllRole();

        void CreateRole(Role role);
        void DeleteRole(int id);
        void EditRole(Role role);
        void SaveRole();
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        public Role GetRole(int roleID)
        {
            return roleRepository.Get(u => u.RoleId == roleID);
        }

        public IEnumerable<Role> GetAllRole()
        {
            var role = roleRepository.GetAll();
            return role;
        }


        public void CreateRole(Role role)
        {
            roleRepository.Add(role);
            SaveRole();
        }
        public void DeleteRole(int id)
        {
            var role = roleRepository.GetById(id);
            roleRepository.Delete(role);
            SaveRole();
        }
        public void EditRole(Role role)
        {
            roleRepository.Update(role);
            SaveRole();
        }
        public void SaveRole()
        {
            unitOfWork.Commit();
        }

    }
}
