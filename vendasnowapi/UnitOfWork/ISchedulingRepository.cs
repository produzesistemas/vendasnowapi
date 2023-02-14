using Models;
using Models.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UnitOfWork
{
    public interface ISchedulingRepository : IDisposable
    {
        Scheduling Get(int id);
        IQueryable<Scheduling> Where(Expression<Func<Scheduling, bool>> expression);
        void Insert(Scheduling entity);
        void CancelByClient(int id);
        void CancelByPartner(int id);
        void Confirm(int id);
        IQueryable<Scheduling> GetPagination(FilterDefault filterDefault);
    }
}
