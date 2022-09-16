using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface ISaleRepository : IDisposable
    {
        Sale Get(int id);
        IQueryable<Sale> Where(Expression<Func<Sale, bool>> expression);
        void Delete(int id);
        void Insert(Sale entity);
        IQueryable<Sale> GetPagination(Expression<Func<Sale, bool>> expression, int sizePage);
    }
}
