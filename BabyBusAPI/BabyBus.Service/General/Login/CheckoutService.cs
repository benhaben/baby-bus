using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using BabyBus.Core.Helper;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Data.Repositories.Main;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Login.Enum;
using BabyBus.Model.Entities.Main;
using BabyBus.Model.Entities.Relation;
using BabyBus.Service.Models;

namespace BabyBus.Service.General.Login
{
    public interface ICheckoutService
    {
        IQueryable<Checkout> GetAllCheckout();
        void CreateCheckout(CheckoutModel model, ref ApiResponser responser);
        void Approve(int id, ref ApiResponser responser);
        void Refuse(int id, string memo);
        void Save();
    }
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository checkoutRepository;
        private readonly IChildRepository childRepository;
        private readonly IParentChildRelationRepository parentChildRelationRepository;
        private readonly ICardPasswordRepository cardPasswordRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CheckoutService(IUnitOfWork unitOfWork, 
            ICheckoutRepository checkoutRepository, 
            IChildRepository childRepository, 
            IParentChildRelationRepository parentChildRelationRepository, 
            ICardPasswordRepository cardPasswordRepository, 
            IUserRepository userRepository) {
            this.unitOfWork = unitOfWork;
            this.checkoutRepository = checkoutRepository;
            this.childRepository = childRepository;
            this.parentChildRelationRepository = parentChildRelationRepository;
            this.cardPasswordRepository = cardPasswordRepository;
            this.userRepository = userRepository;
        }


        public IQueryable<Checkout> GetAllCheckout() {
            return checkoutRepository.GetAll();
        }

        public void CreateCheckout(CheckoutModel model, ref ApiResponser responser) {
            //Check VerifyCode
            if (!string.IsNullOrEmpty(model.VerifyCode)
                && (cardPasswordRepository.Get(u => u.VerifyCode == model.VerifyCode)) == null) {
                responser.Status = false;
                responser.Message = "卡密错误";
                return;
            }

            model.CreateTime = DateTime.Now;
            
            var checkout = new Checkout();
            Mapper.DynamicMap(model, checkout);
            checkoutRepository.Add(checkout);
            Save();
        }

        public void Approve(int id, ref ApiResponser responser) {
            var model = checkoutRepository.GetById(id);
            //Check Exist Parent Child Relation
            if (parentChildRelationRepository.GetAll().Any(x => x.UserId == model.UserId)) {
                responser.Status = false;
                responser.Message = "已存在家长和孩子的绑定关系";
                return;
            }
            CardPassword card = null;
            //Check VerifyCode
            if (!string.IsNullOrEmpty(model.VerifyCode)
                && (card = cardPasswordRepository.Get(u => u.VerifyCode == model.VerifyCode)) == null) {
                responser.Status = false;
                responser.Message = "卡密错误";
                return;
            }
            var user = userRepository.GetById(model.UserId);
            Child child = null;

            using (var scope = new TransactionScope()) {
                var checkout = checkoutRepository.GetById(id);
                checkout.AuditType = AuditType.Passed;
                checkoutRepository.Update(checkout);
                Save();

                user.RealName = model.RealName;
                userRepository.Update(user);
                Save();
                //Free User
                if (string.IsNullOrEmpty(model.VerifyCode)) {
                    if (child == null) {
                        child = new Child();
                        Mapper.DynamicMap(model, child);
                        child.CreateTime = DateTime.Now;
                        childRepository.Add(child);
                        Save();
                    }
                }
                else {
                    if (card.Flag == CardFlag.Pending) {
                        card.Flag = CardFlag.Using;
                        cardPasswordRepository.Update(card);
                        if (child == null) {
                            child = new Child();
                            Mapper.DynamicMap(model, child);
                            childRepository.Add(child);
                            Save();
                        }
                        child.CardPasswordId = card.CardPasswordId;
                        childRepository.Update(child);
                        Save();
                    }
                }
                var parentChildRelation = new ParentChildRelation {
                    ChildId = child.ChildId,
                    UserId = user.UserId
                };
                parentChildRelationRepository.Add(parentChildRelation);
                Save();
                responser.Message = "审批成功";
                scope.Complete();
            }
            
        }

        public void Refuse(int id, string memo) {
            var checkout = checkoutRepository.GetById(id);
            checkout.AuditType = AuditType.Refused;
            checkout.Memo = memo;
            checkoutRepository.Update(checkout);
            Save();
        }

        public void Save() {
            unitOfWork.Commit();
        }
    }
}
