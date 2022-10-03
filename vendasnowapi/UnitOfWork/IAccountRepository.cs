using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface IAccountRepository : IDisposable
    {
        IQueryable<Account> Where(Expression<Func<Account, bool>> expression);
        void Update(Account entity);
        Account Get(int id);
    }
}
