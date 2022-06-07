using Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UnitOfWork
{
    public interface IClientRepository : IDisposable
    {
        Client Get(int id);
        IQueryable<Client> Where(Expression<Func<Client, bool>> expression);
        void Delete(int id);
        void Update(Client entity);
        void Insert(Client entity);


    }
}
