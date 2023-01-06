using Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UnitOfWork
{
    public interface IEstablishmentRepository : IDisposable
    {
        IQueryable<Establishment> GetAll();
        Establishment Get(int id);
        Establishment GetByUser(string id);
        void Active(int id);
        void Delete(int id);
        void Update(Establishment entity);
        void Insert(Establishment entity);


    }
}
