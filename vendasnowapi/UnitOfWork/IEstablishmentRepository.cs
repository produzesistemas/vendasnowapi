using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UnitOfWork
{
    public interface IEstablishmentRepository : IDisposable
    {
        IQueryable<Establishment> GetAll();
        Establishment Get(int id);
        IQueryable<Establishment> Where(Expression<Func<Establishment, bool>> expression);
        void Active(int id);
        void Delete(int id);
        void Update(Establishment entity);
        void Insert(Establishment entity);


    }
}
