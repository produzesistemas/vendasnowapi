using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface ISubscriptionRepository : IDisposable
    {
        Subscription Get(int id);
        IQueryable<Subscription> Where(Expression<Func<Subscription, bool>> expression);
        void Delete(int id);
        void Update(Subscription entity);
        void Insert(Subscription entity);
    }
}
