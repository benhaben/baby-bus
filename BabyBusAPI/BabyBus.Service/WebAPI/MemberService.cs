using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using BabyBus.Core.Helper;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Data.Repositories.Main;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Core.Common;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Model.Entities.Main;
using BabyBus.Model.Entities.Relation;
using BabyBus.Service.Models;
using BabyBus.Service.Models.Enum;

namespace BabyBus.Service.WebAPI
{
    public interface IMemberService
    {
        IEnumerable<string> CityChoice();
        IEnumerable<Kindergarten> ChoiceKindergarten(string city);
        IEnumerable<Class> ChoiceClass(int kindergartenId);
        bool CheckLoginName(string loginName);
        bool CheckPassword(string loginName, string password);
        bool CheckPassword(int userId, string password);
        CardPassword CheckCardPassword(string verifyCode);
        Child CheckChildren(int classId, string childName);
        void Register(string loginName, string password, bool isManager, int weChatInfoId);
        void Register(string loginName, string password, bool isManager);
        int Register(User user);
        void ParendBinding(CheckoutModel model, ref ApiResponser responser);
        void UpdateChildImage(CheckoutModel model);
        void ChangePassword(int userId, string newPassword);
        void Save();

        void UpdateUserInfo(CheckoutModel checkModel);
        void UpdateAll(CheckoutModel model);
    }
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICardPasswordRepository _cardPasswordRepository;
        private readonly IChildRepository _childRepository;
        private readonly IClassRepository _classRepository;
        private readonly IKindergartenRepository _kindergartenRepository;
        private readonly IParentChildRelationRepository _parentChildRelationRepository;

        public MemberService(IUnitOfWork unitOfWork,
            ICardPasswordRepository cardPasswordRepository,
            IUserRepository userRepository,
            IClassRepository classRepository,
            IKindergartenRepository kindergartenRepository,
            IChildRepository childRepository,
            IParentChildRelationRepository parentChildRelationRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _classRepository = classRepository;
            _kindergartenRepository = kindergartenRepository;
            _childRepository = childRepository;
            _parentChildRelationRepository = parentChildRelationRepository;
            _cardPasswordRepository = cardPasswordRepository;
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns>CityList</returns>
        public IEnumerable<string> CityChoice()
        {
            var city = _kindergartenRepository.GetAll().Select(u => u.City).Distinct();
            return city;
        }
        /// <summary>
        /// 获取班级列表
        /// </summary>
        /// <param name="kindergartenId"></param>
        /// <returns></returns>
        public IEnumerable<Class> ChoiceClass(int kindergartenId)
        {
            return _classRepository.GetMany(u => u.KindergartenId == kindergartenId);
        }
        /// <summary>
        /// 获取园区列表
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public IEnumerable<Kindergarten> ChoiceKindergarten(string city)
        {
            return _kindergartenRepository.GetMany(u => u.City == city);
        }
        /// <summary>
        /// 检测LoginName是否存在
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns>true/false</returns>
        public bool CheckLoginName(string loginName)
        {
            var userLogin = _userRepository.Get(u => u.LoginName == loginName);
            return userLogin != null;
        }
        /// <summary>
        /// 依据LoginName检测Password是否正确
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckPassword(string loginName, string password)
        {
            var userLogin = _userRepository.Get(u => u.LoginName == loginName);
            return userLogin.Password != password;
        }
        /// <summary>
        /// 依据UserId检测Password是否正确
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns>true/false</returns>
        public bool CheckPassword(int userId, string password)
        {
            var userLogin = _userRepository.GetById(userId);
            if (userLogin.Password == password)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检测卡片是否用过
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public CardPassword CheckCardPassword(string verifyCode)
        {
            var cardPasswordInfo = _cardPasswordRepository.Get(u => u.VerifyCode == verifyCode);
            return cardPasswordInfo;

        }
        /// <summary>
        /// 微信用户注册
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="isManager"></param>
        /// <param name="weChatInfoId"></param>
        public void Register(string loginName, string password, bool isManager,int weChatInfoId)
        {
            var user = new User
            {
                LoginName = loginName,
                Password = password,
                WeChatInfoId = weChatInfoId
            };
            _userRepository.Add(user);
            Save();
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="isManager"></param>
        public void Register(string loginName, string password, bool isManager)
        {
            var user = new User
            {
                LoginName = loginName,
                Password = password
            };
            _userRepository.Add(user);
            Save();
        }

        public int Register(User user)
        {
            _userRepository.Add(user);
            Save();
            return user.UserId;
        }

        public Child CheckChildren(int classId,string childName)
        {
           return _childRepository.GetAll().Where(u => u.ClassId == classId)
                .FirstOrDefault(u => u.ChildName == childName);
        }
        /// <summary>
        /// 免费用户绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="kindergartenId"></param>
        /// <param name="parentName"></param>
        /// <param name="childModel"></param>
//        public void ParentBinding(int userId, int kindergartenId, string parentName, Child childModel)
//        {
//            var userInfo = _userRepository.GetById(userId);
//            var pcRelation = _parentChildRelationRepository.Get(u => u.ParentId == userInfo.ParentId);
//            using(var scope = new TransactionScope())
//            {
//                var parent = new Parent();
//                if (userInfo.ParentId == 0)
//                {
//                    parent.ParentName = parentName;
//                    parent.IsMessagePush = false;
//                    _parentRepository.Add(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                else
//                {
//                    parent.ParentName = parentName;
//                    _parentRepository.Add(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                //添加孩子
//                var child = new Child
//                {
//                    CardPasswordId = 0,
//                    ChildName = childModel.ChildName,
//                    ClassId = childModel.ClassId,
//                    Gender = childModel.Gender,
//                    Birthday = childModel.Birthday,
//                    CreateTime = DateTime.Now
//                };
//                _childRepository.Add(child);
//                Save();
//                //添加[孩子-家长]对应关系
//                var pcr = new ParentChildRelation();
//                if (pcRelation == null)
//                {
//                    pcr.ChildId = child.ChildId;
//                    pcr.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Add(pcr);
//                    Save();
//                }
//                else
//                {
//                    pcRelation.ChildId = child.ChildId;
//                    pcRelation.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Update(pcRelation);
//                    Save();
//                }
//
//                scope.Complete();
//            }
//        }
//
//        /// <summary>
//        /// 免费用户绑定已有孩子
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="childId"></param>
//        /// <param name="parentName"></param>
//        public void ParentBinding(int userId, int childId, string parentName)
//        {
//            var userInfo = _userRepository.GetById(userId);
//            var pcRelation = _parentChildRelationRepository.Get(u => u.ParentId == userInfo.ParentId);
//            using (var scope = new TransactionScope())
//            {
//                var parent = new Parent();
//                if (userInfo.ParentId == 0)
//                {
//                    parent.ParentName = parentName;
//                    parent.IsMessagePush = false;
//                    _parentRepository.Add(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                else
//                {
//                    parent.ParentName = parentName;
//                    _parentRepository.Update(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                //添加[孩子-家长]对应关系
//                var pcr = new ParentChildRelation();
//                if (pcRelation == null)
//                {
//                    pcr.ChildId = childId;
//                    pcr.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Add(pcr);
//                    Save();
//                }
//                else
//                {
//                    pcr.ChildId = childId;
//                    pcr.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Update(pcr);
//                    Save();
//                }
//                scope.Complete();
//            }
//        }

        public void ParendBinding(CheckoutModel model,ref ApiResponser responser) {
            model.CreateTime = DateTime.Now;
            //Check Exist Parent Child Relation
            if (_parentChildRelationRepository.GetAll().Any(x => x.UserId == model.UserId))
            {
                responser.Status = false;
                responser.Message = "已存在家长和孩子的绑定关系";
                return;
            }
            CardPassword card = null;
            //Check VerifyCode
            if (!string.IsNullOrEmpty(model.VerifyCode)
                && (card = _cardPasswordRepository.Get(u => u.VerifyCode == model.VerifyCode)) == null)
            {
                responser.Status = false;
                responser.Message = "卡密错误";
                return;
            }
            var user = _userRepository.GetById(model.UserId);
            //TODO:暂时不考虑多个家长绑定到一个孩子上
            //var child = _childRepository.GetAll().FirstOrDefault(x => x.ChildName == model.ChildName);
            Child child = null;

            using (var scope = new TransactionScope())
            {
                user.RealName = model.RealName;
                _userRepository.Update(user);
                Save();
                //Free User
                if (string.IsNullOrEmpty(model.VerifyCode))
                {
                    if (child == null)
                    {
                        child = new Child();
                        Mapper.DynamicMap(model, child);
                        child.CreateTime = DateTime.Now;
                        _childRepository.Add(child);
                        Save();
                    }
                }
                else
                {
                    if (card.Flag == CardFlag.Pending)
                    {
                        card.Flag = CardFlag.Using;
                        _cardPasswordRepository.Update(card);
                        if (child == null)
                        {
                            child = new Child();
                            Mapper.DynamicMap(model, child);
                            _childRepository.Add(child);
                            Save();
                        }
                        child.CardPasswordId = card.CardPasswordId;
                        _childRepository.Update(child);
                        Save();
                    }
                }
                var parentChildRelation = new ParentChildRelation
                {
                    ChildId = child.ChildId,
                    UserId = user.UserId
                };
                _parentChildRelationRepository.Add(parentChildRelation);
                Save();
                responser.Message = "您与孩子已经绑定成功";
                scope.Complete();
            }
        }

        public void UpdateChildImage(CheckoutModel model)
        {
            var child = _childRepository.GetById(model.ChildId);
            if (child != null)
            {
                child.ImageName = model.ImageName;
                _childRepository.Update(child);
                Save();
            }
        }

        /// <summary>
        /// 绑定未开卡用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="kindergartenId"></param>
        /// <param name="parentName"></param>
        /// <param name="verifyCode"></param>
        /// <param name="childModel"></param>
//        public void ParentBinding(int userId, int kindergartenId, string parentName,string verifyCode, Child childModel)
//        {
//            var userInfo = _userRepository.GetById(userId);
//            var cardPasswordInfo = _cardPasswordRepository.Get(u => u.VerifyCode == verifyCode);
//            var pcRelation = _parentChildRelationRepository.Get(u => u.ParentId == userInfo.ParentId);
//            using(var scope = new TransactionScope())
//            {
//                //卡密变更为已使用
//                cardPasswordInfo.Flag = CardFlag.Using;
//                cardPasswordInfo.CreateTime = DateTime.Now;
//                _cardPasswordRepository.Update(cardPasswordInfo);
//                Save();
//                //添加家长&更新User表
//                var parent = new Parent();
//                if (userInfo.ParentId == 0)
//                {
//                    parent.ParentName = parentName;
//                    parent.IsMessagePush = true;
//                    _parentRepository.Add(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                else
//                {
//                    parent.ParentName = parentName;
//                    _parentRepository.Update(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//
//                }
//                //添加孩子
//                var child = new Child
//                {
//                    CardPasswordId = cardPasswordInfo.CardPasswordId,
//                    ChildName = childModel.ChildName,
//                    ClassId = childModel.ClassId,
//                    Gender = childModel.Gender,
//                    Birthday = childModel.Birthday,
//                    CreateTime = DateTime.Now
//                };
//                _childRepository.Add(child);
//                Save();
//                //添加[孩子-家长]对应关系
//                var pcr = new ParentChildRelation();
//                if (pcRelation == null)
//                {
//                    pcr.ChildId = child.ChildId;
//                    pcr.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Add(pcr);
//                    Save();
//                }
//                else
//                {
//                    pcRelation.ChildId = child.ChildId;
//                    pcRelation.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Update(pcRelation);
//                    Save();
//                }
//                scope.Complete();
//            }
//        }
//        /// <summary>
//        /// 绑定已开卡用户
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="verifyCode"></param>
//        /// <param name="parentName"></param>
//        public void ParentBinding(int userId, string verifyCode, string parentName)
//        {
//            var userInfo = _userRepository.GetById(userId);
//            var cardPasswordInfo = _cardPasswordRepository.Get(u => u.VerifyCode == verifyCode);
//            var pcRelation = _parentChildRelationRepository.Get(u => u.ParentId == userInfo.ParentId);
//            var childId = _childRepository.Get(u => u.CardPasswordId == cardPasswordInfo.CardPasswordId).ChildId;
//            using (var scope = new TransactionScope())
//            {
//                var parent = new Parent();
//                if (userInfo.ParentId == 0)
//                {
//                    parent.ParentName = parentName;
//                    parent.IsMessagePush = true;
//                    _parentRepository.Add(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                else
//                {
//                    parent.ParentName = parentName;
//                    _parentRepository.Update(parent);
//                    Save();
//                    userInfo.ParentId = parent.ParentId;
//                    _userRepository.Update(userInfo);
//                    Save();
//                }
//                //添加[孩子-家长]对应关系
//                var pcr = new ParentChildRelation();
//                if (pcRelation == null)
//                {
//                    pcr.ChildId = childId;
//                    pcr.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Add(pcr);
//                    Save();
//                }
//                else
//                {
//                    pcr.ChildId = childId;
//                    pcr.ParentId = parent.ParentId;
//                    _parentChildRelationRepository.Update(pcr);
//                    Save();
//                }
//                scope.Complete();
//            }
//        }
//        /// <summary>
//        /// 绑定老师/园长
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="classId"></param>
//        /// <param name="kindergartenId"></param>
//        /// <param name="roleId"></param>
//        /// <param name="createTime"></param>
//        /// <param name="managerName"></param>
//        public void ManagerBinding(int userId, int classId, int kindergartenId, int roleId, int createTime, string managerName)
//        {
//            var userInfo = _userRepository.GetById(userId);
//            if (userInfo.ManagerId == 0)
//            {
//                using (var scope = new TransactionScope())
//                {
//                    //增加老师信息
//                    var manager = new Manager {ManagerName = managerName};
//                    _managerRepository.Add(manager);
//                    Save();
//                    //更新User表
//                    userInfo.ManagerId = manager.ManagerId;
//                    userInfo.IsManager = false;
//                    _userRepository.Update(userInfo);
//                    Save();
//                    //添加[老师-权限]对应关系
//                    var managerRole = new ManagerRoleRelation {ManagerId = manager.ManagerId, RoleId = roleId};
//                    _managerRoleRelationRepository.Add(managerRole);
//                    Save();
//                    if (roleId == 1)
//                    {
//                        //添加[老师-班级]对应关系
//                        var managerClass = new ManagerClassRelation {ClassId = classId, ManagerId = manager.ManagerId};
//                        _managerClassRelationRepository.Add(managerClass);
//                        Save();
//                    }
//                    else
//                    {
//                        var managerKindergarten = new ManagerKindergartenRelation
//                        {
//                            KindergartenId = kindergartenId,
//                            ManagerId = manager.ManagerId
//                        };
//                        _managerKindergartenRelationRepository.Add(managerKindergarten);
//                        Save();
//                    }
//                    scope.Complete();
//                }
//            }
//        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        public void ChangePassword(int userId,string newPassword)
        {
            var userInfo = _userRepository.GetById(userId);
            userInfo.Password = newPassword;
            Save();
        }

        /// <summary>
        /// SaveChange
        /// </summary>
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUserInfo(CheckoutModel checkModel)
        {
            if (checkModel.CheckoutType == CheckoutType.UpdateParentName)
            {
                var user = _userRepository.GetById(checkModel.UserId);
                user.RealName = checkModel.RealName;
                _userRepository.Update(user);
                Save();
            }
            else
            {
                var child = _childRepository.GetById(checkModel.ChildId);
                switch (checkModel.CheckoutType)
                {
                      case CheckoutType.UpdateChildName:
                        child.ChildName = checkModel.ChildName;
                        break;
                        case CheckoutType.UpdateChildGender:
                        child.Gender = checkModel.Gender;
                        break;
                        case CheckoutType.UpdateChildBirthday:
                        child.Birthday = checkModel.Birthday;
                        break;                    
                }
                _childRepository.Update(child);
                Save();
            }
        }

        public void UpdateAll(CheckoutModel model)
        {
            if (model.RealName != Constant.StringNull)
            {
                var user = _userRepository.GetById(model.UserId);
                user.RealName = model.RealName;
                _userRepository.Update(user);
                Save();
            }
            if (model.RoleType == RoleType.Parent)
            {
                if (model.ChildId > 0)
                {
                    var child = _childRepository.GetById(model.ChildId);
                    if (model.HasHeadImage)
                    {
                        child.ImageName = model.ImageName;
                    }
                    if (model.ChildName != Constant.StringNull)
                    {
                        child.ChildName = model.ChildName;
                    }
                    if (model.Gender != Constant.IntNull)
                    {
                        child.Gender = model.Gender;
                    }
                    if (model.Birthday != Constant.DateNull)
                    {
                        child.Birthday = model.Birthday;
                    }
                    _childRepository.Update(child);
                    Save();
                }
            }
            else
            {
                var user = _userRepository.GetById(model.UserId);
                if (model.HasHeadImage)
                {
                    user.ImageName = model.ImageName;
                }
                _userRepository.Update(user);
                Save();
            }

        }
    }
}
