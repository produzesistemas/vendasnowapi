using Models;
using System.Linq;
using UnitOfWork;
using System.Linq.Expressions;
using System;
using Microsoft.EntityFrameworkCore;

namespace Repositorys
{
    public class SaleRepository : ISaleRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var entity = _context.Sale.Single(x => x.Id == id);
            var accounts = _context.Account.Where(c => c.SaleId == id);
            var products = _context.SaleProduct.Where(c => c.SaleId == id);
            var services = _context.SaleService.Where(c => c.SaleId == id);
            _context.RemoveRange(accounts);
            _context.RemoveRange(products);
            _context.RemoveRange(services);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void Insert(Sale entity)
        {
            _context.Sale.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<Sale> Where(Expression<Func<Sale, bool>> expression)
        {
            return _context.Sale
                .Include(x => x.PaymentCondition)
                .Include(x => x.Account)
                .Include(x => x.Client)
                .Include(x => x.SaleProduct).ThenInclude(x => x.Product)
                .Include(x => x.SaleService)
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

        public IQueryable<Sale> GetPagination(Expression<Func<Sale, bool>> expression, int sizePage)
        {
            return _context.Sale.Where(expression).Take(sizePage);
        }
    }
}
