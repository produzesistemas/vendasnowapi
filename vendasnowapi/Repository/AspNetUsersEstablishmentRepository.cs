using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace Repositorys
{
    public class AspNetUsersEstablishmentRepository : IAspNetUsersEstablishmentRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public AspNetUsersEstablishmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public AspNetUsersEstablishment Get(int id)
        {
            return _context.AspNetUsersEstablishment
                .Include(x => x.Establishment)
                .Include(x => x.ApplicationUser)
                .Single(x => x.Id == id);
        }

        public IQueryable<AspNetUsersEstablishment> Where(Expression<Func<AspNetUsersEstablishment, bool>> expression)
        {
            return _context.AspNetUsersEstablishment.Where(expression)
                .Include(x => x.Establishment)
                .Include(x => x.ApplicationUser)
                 .AsQueryable();
        }

        public void Delete(int id)
        {
            var entity = _context.AspNetUsersEstablishment.Single(x => x.Id == id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void Insert(AspNetUsersEstablishment entity)
        {
            _context.AspNetUsersEstablishment.Add(entity);
            _context.SaveChanges();
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

        public AspNetUsersEstablishment GetByUser(string id)
        {
            return _context.AspNetUsersEstablishment
               .Single(x => x.AspNetUsersId == id);
        }
    }
}

