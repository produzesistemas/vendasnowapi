using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface IServiceRepository : IDisposable
    {
        Service Get(int id);
        IQueryable<Service> Where(Expression<Func<Service, bool>> expression);
        void Delete(int id);
        void Update(Service entity, string fileDelete);
        void Insert(Service entity);
        void Active(int id);
    }
}
