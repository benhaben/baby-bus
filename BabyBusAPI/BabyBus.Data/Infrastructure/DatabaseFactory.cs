namespace BabyBus.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private BabyBusDataContext _dataContext;
        public BabyBusDataContext Get()
        {
            return _dataContext ?? (_dataContext = new BabyBusDataContext());
        }
        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
