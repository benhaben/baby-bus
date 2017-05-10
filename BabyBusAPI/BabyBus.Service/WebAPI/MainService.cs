using System.Collections.Generic;
using System.Linq;
using BabyBus.Data.Repositories.Login;
using BabyBus.Data.Repositories.Main;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Main;

namespace BabyBus.Service.WebAPI
{
    public interface IMainService
    {
        Kindergarten GetKindergarten(int userId);
        Class GetClass(int userId);
        IEnumerable<Child> GetChildList(int userId);
        IEnumerable<Kindergarten> GetKindergartenList(int userId);
        IEnumerable<Class> GetClassList(int userId);
    }
    public class MainService : IMainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IChildRepository _childRepository;
        private readonly IClassRepository _classRepository;
        private readonly IParentChildRelationRepository _parentChildRelationRepository;
        private readonly IKindergartenRepository _kindergartenRepository;

        public MainService(IUserRepository userRepository,
            IChildRepository childRepository,
            IClassRepository classRepository,
            IParentChildRelationRepository parentChildRelationRepository,
            IKindergartenRepository kindergartenRepository)
        {
            _userRepository = userRepository;
            _childRepository = childRepository;
            _classRepository = classRepository;
            _parentChildRelationRepository = parentChildRelationRepository;
            _kindergartenRepository = kindergartenRepository;
        }

        /// <summary>
        /// 园长和老师依据UserId获取孩子列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ChildList</returns>
        public IEnumerable<Child> GetChildList(int userId)
        {
//            var userInfo = _userRepository.GetById(userId);
//            var managerRole = _managerRoleRelationRepository.Get(u => u.ManagerId == userInfo.ManagerId);
//            if (managerRole.RoleId == 1)
//            {
//                return _childRepository.GetMany(u => u.ClassId == GetClass(userId).ClassId);
//            }
//            if (managerRole.RoleId > 1)
//            {
//                var childlst = new List<Child>();
//                foreach(var item in GetClassList(userId))
//                {
//                    var citem = item;
//                    childlst.AddRange(_childRepository.GetMany(u => u.ClassId == citem.ClassId));
//                }
//                return childlst;
//            }
            return null;
 
        }
        /// <summary>
        /// 老师 家长 园长依据UserId获取园区信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Kindergarten Information</returns>
        public Kindergarten GetKindergarten(int userId)
        {
//            var userInfo = _userRepository.GetById(userId);
//            if(userInfo.IsManager)
//            {
//                var managerRole = _managerRoleRelationRepository.Get(u => u.ManagerId == userInfo.ManagerId);
//                if(managerRole.RoleId == 1)
//                {
//                    var managerClass = _managerClassRelationRepository.Get(u => u.ManagerId == userInfo.ManagerId);
//                    var classInfo = _classRepository.GetById(managerClass.ClassId);
//                    return _kindergartenRepository.GetById(classInfo.KindergartenId);
//                }
//                if (managerRole.RoleId == 2)
//                {
//                    var managerKindergarten = _managerKindergartenRelationRepository.Get(u => u.ManagerId == userInfo.ManagerId);
//                    return _kindergartenRepository.GetById(managerKindergarten.KindergartenId);
//                }
//            }
//            else
//            {
//                var parentChild = _parentChildRelationRepository.Get(u => u.ParentId == userInfo.ParentId);
//                var childInfo = _childRepository.GetById(parentChild.ChildId);
//                var classInfo = _classRepository.GetById(childInfo.ClassId);
//                return _kindergartenRepository.GetById(classInfo.KindergartenId);
//            }
            return null;
        }
        /// <summary>
        /// 总园长 管理员获取园区列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>KindergartenList</returns>
        public IEnumerable<Kindergarten> GetKindergartenList(int userId)
        {
//            var userInfo = _userRepository.GetById(userId);
//            var managerKindergarten = _managerKindergartenRelationRepository.GetMany(u => u.ManagerId == userInfo.ManagerId);
//            var kindergartenlst = new List<Kindergarten>();
//            foreach(var item in managerKindergarten)
//            {
//                kindergartenlst.Add(_kindergartenRepository.GetById(item.KindergartenId));
//            }
//            return kindergartenlst;
            return null;
        }
        /// <summary>
        /// 家长和老师权限获取班级信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Class Information</returns>
        public Class GetClass(int userId)
        {
            return _classRepository.Get(u => u.KindergartenId == GetKindergarten(userId).KindergartenId);
        }
        /// <summary>
        /// 总园长 管理员获取班级列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ClassList</returns>
        public IEnumerable<Class> GetClassList(int userId)
        {
//            var userInfo = _userRepository.GetById(userId);
//            var managerRole = _managerRoleRelationRepository.Get(u => u.ManagerId == userInfo.ManagerId);
//            if (managerRole.RoleId == 2)
//            {
//                var managerKindergarten = _managerKindergartenRelationRepository.Get(u => u.ManagerId == userInfo.ManagerId);
//                return _classRepository.GetMany(u => u.KindergartenId == managerKindergarten.KindergartenId);
//            }
//            if (managerRole.RoleId > 2)
//            {
//                var managerKindergarten = _managerKindergartenRelationRepository.GetMany(u => u.ManagerId == userInfo.ManagerId);
//                var classlst = new List<Class>();
//                foreach (var item in managerKindergarten)
//                {
//                    var mitem = item;
//                    classlst.AddRange(_classRepository.GetMany(u => u.KindergartenId == mitem.KindergartenId));
//                }
//                return classlst;
//            }
            return null;
        }
    }
}
