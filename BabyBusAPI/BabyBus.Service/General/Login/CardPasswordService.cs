using System.Collections.Generic;
using BabyBus.Data.Infrastructure;
using BabyBus.Data.Repositories.Login;
using BabyBus.Model.Entities.Login;

namespace BabyBus.Service.General
{
    public interface ICardPasswordService
    {
        CardPassword GetCardPassword(int cardPasswordID);
        IEnumerable<CardPassword> GetAllCardPassword();

        void CreateCardPassword(CardPassword cardPassword);
        void DeleteCardPassword(int id);
        void EditCardPassword(CardPassword cardPassword);
        void SaveCardPassword();
    }
    public class CardPasswordService : ICardPasswordService
    {
        private readonly ICardPasswordRepository cardPasswordsRepository;
        private readonly IUnitOfWork unitOfWork;

        public CardPasswordService(ICardPasswordRepository cardPasswordsRepository, IUnitOfWork unitOfWork)
        {
            this.cardPasswordsRepository = cardPasswordsRepository;
            this.unitOfWork = unitOfWork;
        }

        public CardPassword GetCardPassword(int cardPasswordID)
        {
            return cardPasswordsRepository.Get(u => u.CardPasswordId == cardPasswordID);
        }

        public IEnumerable<CardPassword> GetAllCardPassword()
        {
            var cardPasswords = cardPasswordsRepository.GetAll();
            return cardPasswords;
        }


        public void CreateCardPassword(CardPassword cardPasswords)
        {
            cardPasswordsRepository.Add(cardPasswords);
            SaveCardPassword();
        }
        public void DeleteCardPassword(int id)
        {
            var cardPasswords = cardPasswordsRepository.GetById(id);
            cardPasswordsRepository.Delete(cardPasswords);
            SaveCardPassword();
        }
        public void EditCardPassword(CardPassword cardPassword)
        {
            cardPasswordsRepository.Update(cardPassword);
            SaveCardPassword();
        }
        public void SaveCardPassword()
        {
            unitOfWork.Commit();
        }

    }
}
