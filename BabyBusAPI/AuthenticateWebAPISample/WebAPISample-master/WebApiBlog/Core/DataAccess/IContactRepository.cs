using System;
using WebApiBlog.Models;

namespace WebApiBlog.Core.DataAccess
{
    public interface IContactRepository : IRepository<Contact, Guid>
    {

    }
}