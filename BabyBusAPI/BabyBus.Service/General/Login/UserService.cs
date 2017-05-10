using System.Collections.Generic;
using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Data.Repositories.Main;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Model.Entities.Main;
using BabyBus.Service.Models;

namespace BabyBus.Service.General
{
    public interface IUserService
    {
        User GetUser(int userID);
        IQueryable<User> GetAllUser();
        void CreateUser(User user);
        void DeleteUser(int id);
        void EditUser(User user);
        void SaveUser();

        LoginCheckStatus CheckLoginUser(User loginUser);
        User GetUserByLoginName(string loginName);
        Child GetChildByUserId(int userId);
        Class GetClassById(int classId);
        Kindergarten GetKindergartenById(int kindergartenId);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IChildRepository childRepository;
        private readonly IKindergartenRepository kindergartenRepository;
        private readonly IClassRepository classRepository;
        private readonly IParentChildRelationRepository pcRelationRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork,
            IChildRepository childRepository,IKindergartenRepository kindergartenRepository,
            IClassRepository classRepository,IParentChildRelationRepository pcRelationRepository)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.childRepository = childRepository;
            this.kindergartenRepository = kindergartenRepository;
            this.classRepository = classRepository;
            this.pcRelationRepository = pcRelationRepository;
        }

        public User GetUser(int userID)
        {
            return userRepository.Get(u => u.UserId == userID);
        }

        public IQueryable<User> GetAllUser()
        {
            var user = userRepository.GetAll();
            return user;
        }


        public void CreateUser(User user)
        {
            userRepository.Add(user);
            SaveUser();
        }
        public void DeleteUser(int id)
        {
            var user = userRepository.GetById(id);
            userRepository.Delete(user);
            SaveUser();
        }
        public void EditUser(User user)
        {
            userRepository.Update(user);
            SaveUser();
        }
        public void SaveUser()
        {
            unitOfWork.Commit();
        }

        public LoginCheckStatus CheckLoginUser(User loginUser)
        {
            if(userRepository.Get(u => u.LoginName == loginUser.LoginName && u.Password == loginUser.Password) == null)
                return LoginCheckStatus.UnCorrect;
            return LoginCheckStatus.Correct;
        }

        public User GetUserByLoginName(string loginName)
        {
            return userRepository.Get(u => u.LoginName == loginName);
        }

        public Child GetChildByUserId(int userId)
        {
            var pc = pcRelationRepository.Get(x => x.UserId == userId);
            if (pc == null)
                return null;
            var child = childRepository.GetById(pc.ChildId);
            //var child = childRepository.Get(x => x.ChildId == pc.ChildId);
            if (child == null)
                return null;
            return child;
        }

        public Class GetClassById(int classId)
        {
            return classRepository.GetById(classId);
        }

        public Kindergarten GetKindergartenById(int kindergartenId)
        {
            return kindergartenRepository.GetById(kindergartenId);
        }
    }
}
