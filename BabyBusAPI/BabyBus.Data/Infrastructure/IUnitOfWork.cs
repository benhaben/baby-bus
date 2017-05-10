namespace BabyBus.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
