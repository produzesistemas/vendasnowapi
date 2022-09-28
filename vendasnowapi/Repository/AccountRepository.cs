using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace Repositorys
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Update(Account entity)
        {
            var entityBase = _context.Account.Single(x => x.Id == entity.Id);
            if (entity.DateOfPayment.HasValue) { entityBase.DateOfPayment = entity.DateOfPayment.Value; }
            if (entity.AmountPaid.HasValue) { entityBase.AmountPaid = entity.AmountPaid.Value; }
            entityBase.Status = entity.Status;
            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<Account> Where(Expression<Func<Account, bool>> expression)
        {
            return _context.Account
                .Include(x => x.Sale)
                .ThenInclude(x => x.Client)
                .Include(x => x.Sale)
                .ThenInclude(x => x.PaymentCondition)
                .Where(expression).AsQueryable();
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

        public IQueryable<Client> GetPagination(Expression<Func<Client, bool>> expression, int sizePage)
        {
            return _context.Client.Where(expression).Take(sizePage);
        }
    }
}
