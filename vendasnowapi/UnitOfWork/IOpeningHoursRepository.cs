using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface IOpeningHoursRepository : IDisposable
    {
        OpeningHours Get(int id);
        IQueryable<OpeningHours> Where(Expression<Func<OpeningHours, bool>> expression);
        void Delete(int id);
        void Update(OpeningHours entity);
        void Insert(OpeningHours entity);
        void Active(int id);
    }
}
