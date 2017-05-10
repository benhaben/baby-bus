using System.Linq;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.FAQ;
using BabyBus.Model.Entities.FAQ;

namespace BabyBus.Service.General.FAQ {
    public interface IQuestionService {
        Question GetQuestion(int questionId);
        IQueryable<Question> GetAllQuestion();

        void CreateQuestion(Question question);
        void DeleteQuestion(int id);
        void EditQuestion(Question question);
        void SaveQuestion();
    }

    public class QuestionService : IQuestionService {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(IQuestionRepository questionRepository, IUnitOfWork unitOfWork) {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public Question GetQuestion(int questionId) {
            return _questionRepository.Get(u => u.QuestionId == questionId);
        }

        public IQueryable<Question> GetAllQuestion() {
            IQueryable<Question> question = _questionRepository.GetAll();
            return question;
        }

        public void CreateQuestion(Question question) {
            _questionRepository.Add(question);
            SaveQuestion();
        }

        public void DeleteQuestion(int id) {
            Question question = _questionRepository.GetById(id);
            _questionRepository.Delete(question);
            SaveQuestion();
        }

        public void EditQuestion(Question question) {
            _questionRepository.Update(question);
            SaveQuestion();
        }

        public void SaveQuestion() {
            _unitOfWork.Commit();
        }
    }
}