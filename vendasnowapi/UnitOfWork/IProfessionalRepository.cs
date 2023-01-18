using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface IProfessionalRepository : IDisposable
    {
        Professional Get(int id);
        IQueryable<Professional> Where(Expression<Func<Professional, bool>> expression);
        void Delete(int id);
        void Update(Professional entity, string fileDelete);
        void Insert(Professional entity);
        void Active(int id);
    }
}
