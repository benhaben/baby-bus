using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Relation;
using BabyBus.Model.Entities.Relation;

namespace BabyBus.Service.General
{
    public interface IParentChildRelationService
    {
        ParentChildRelation GetParentChildRelation(int parentChildRelationID);
        IEnumerable<ParentChildRelation> GetAllParentChildRelation();

        void CreateParentChildRelation(ParentChildRelation parentChildRelation);
        void DeleteParentChildRelation(int id);
        void EditParentChildRelation(ParentChildRelation parentChildRelation);
        void SaveParentChildRelation();
    }
    public class ParentChildRelationService : IParentChildRelationService
    {
        private readonly IParentChildRelationRepository userRoleRelationRepository;
        private readonly IUnitOfWork unitOfWork;

        public ParentChildRelationService(IParentChildRelationRepository userRoleRelationRepository, IUnitOfWork unitOfWork)
        {
            this.userRoleRelationRepository = userRoleRelationRepository;
            this.unitOfWork = unitOfWork;
        }

        public ParentChildRelation GetParentChildRelation(int parentChildRelationID)
        {
            return userRoleRelationRepository.Get(u => u.ParentChildRelationId == parentChildRelationID);
        }

        public IEnumerable<ParentChildRelation> GetAllParentChildRelation()
        {
            var parentChildRelation = userRoleRelationRepository.GetAll();
            return parentChildRelation;
        }


        public void CreateParentChildRelation(ParentChildRelation parentChildRelation)
        {
            userRoleRelationRepository.Add(parentChildRelation);
            SaveParentChildRelation();
        }
        public void DeleteParentChildRelation(int id)
        {
            var parentChildRelation = userRoleRelationRepository.GetById(id);
            userRoleRelationRepository.Delete(parentChildRelation);
            SaveParentChildRelation();
        }
        public void EditParentChildRelation(ParentChildRelation parentChildRelation)
        {
            userRoleRelationRepository.Update(parentChildRelation);
            SaveParentChildRelation();
        }
        public void SaveParentChildRelation()
        {
            unitOfWork.Commit();
        }

    }
}
