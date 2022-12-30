using Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UnitOfWork
{
    public interface IAspNetUsersEstablishmentRepository : IDisposable
    {
        AspNetUsersEstablishment Get(int id);
        AspNetUsersEstablishment GetByUser(string id);
        IQueryable<AspNetUsersEstablishment> Where(Expression<Func<AspNetUsersEstablishment, bool>> expression);
        void Delete(int id);
        void Insert(AspNetUsersEstablishment entity);
    }
}
