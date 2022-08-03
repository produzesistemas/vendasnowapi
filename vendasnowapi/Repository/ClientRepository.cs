using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace Repositorys
{
    public class ClientRepository : IClientRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var entity = _context.Client.Single(x => x.Id == id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Client Get(int id)
        {
            return _context.Client.Single(x => x.Id == id);
        }

        public void Insert(Client entity)
        {
            _context.Client.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Client entity)
        {
            var entityBase = _context.Client.Single(x => x.Id == entity.Id);
            entityBase.Name = entity.Name;
            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<Client> Where(Expression<Func<Client, bool>> expression)
        {
            return _context.Client.Where(expression).AsQueryable();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
