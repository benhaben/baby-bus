using System.Collections.Generic;
using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.FAQ;
using BabyBus.Model.Entities.FAQ;

namespace BabyBus.Service.General
{
    public interface IAnswerService
    {
        Answer GetAnswer(int answerID);
        IQueryable<Answer> GetAllAnswer();
        void CreateAnswer(Answer answer);
        void DeleteAnswer(int id);
        void EditAnswer(Answer answer);     
        void SaveAnswer();
    }
    public class AnswerService:IAnswerService
    {
        private readonly IAnswerRepository answerRepository;
        private readonly IUnitOfWork unitOfWork;

        public AnswerService(IAnswerRepository answerRepository, IUnitOfWork unitOfWork)
        {
            this.answerRepository = answerRepository;
            this.unitOfWork = unitOfWork;
        }

        public Answer GetAnswer(int answerID)
        {
            return answerRepository.Get(u => u.AnswerId == answerID);
        }

        public IQueryable<Answer> GetAllAnswer()
        {
            var answer = answerRepository.GetAll();
            return answer;
        }


        public void CreateAnswer(Answer answer)
        {
            answerRepository.Add(answer);
            SaveAnswer();
        }
        public void DeleteAnswer(int id)
        {
            var answer = answerRepository.GetById(id);
            answerRepository.Delete(answer);
            SaveAnswer();
        }
        public void EditAnswer(Answer answer)
        {
            answerRepository.Update(answer);
            SaveAnswer();
        }
        public void SaveAnswer()
        {
            unitOfWork.Commit();
        }
        
    }
}
