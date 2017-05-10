using System.Collections.Generic;

namespace AdministratorManagement.Core.DataAccess
{
    public interface IRepository<T, in TId>
    {
        T FindById(TId id);
        IEnumerable<T> FindAll();
        void Save(T entity);
        void Update(T entity, TId id);
        void Delete(T entity);
        
    }
}