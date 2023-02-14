using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class SchedulingEmailRepository : ISchedulingEmailRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public SchedulingEmailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Insert(SchedulingEmail entity)
        {
            _context.SchedulingEmail.Add(entity);
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

    }
}
