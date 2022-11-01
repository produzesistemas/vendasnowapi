using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class SubscriptionRepository : ISubscriptionRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var entity = _context.Subscription.Single(x => x.Id == id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Subscription Get(int id)
        {
            return _context.Subscription.Single(x => x.Id == id);
        }

        public void Insert(Subscription entity)
        {
            _context.Subscription.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Subscription entity)
        {
            var entityBase = _context.Subscription.Single(x => x.Id == entity.Id);
            entityBase.Active = entity.Active;
            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<Subscription> Where(Expression<Func<Subscription, bool>> expression)
        {
            return _context.Subscription.Where(expression).AsQueryable();
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

        public Subscription GetCurrent(Expression<Func<Subscription, bool>> expression)
        {
            var subs = _context.Subscription.Include(x => x.Plan).Where(expression).OrderByDescending(x => x.SubscriptionDate);
            return subs.FirstOrDefault();
        }
    }
}
