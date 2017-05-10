using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace BabyBus.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private BabyBusDataContext _dataContext;
        private readonly IDbSet<T> _dbset;
        private IObjectContextAdapter _context;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected BabyBusDataContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            //RemoveHoldingEntityInContext(entity);
            //_dataContext.Entry(entity).State = EntityState.Unchanged;
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }
        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }
        public virtual IQueryable<T> GetAll()
        {
            return _dbset.AsQueryable();
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> @where)
        {
            return _dbset.Where(where);
            //return _dbset.Where(where).AsNoTracking();
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault();
            //return _dbset.Where(where).AsNoTracking().FirstOrDefault();
        }
    }
}
