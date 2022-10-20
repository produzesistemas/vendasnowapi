using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface IPlanRepository : IDisposable
    {
        Plan Get(int id);
        IQueryable<Plan> Where(Expression<Func<Plan, bool>> expression);
    }
}
