using Models;
using System.Linq;
using UnitOfWork;
using System.Linq.Expressions;
using System;
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
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Sale Get(int id)
        {
            return _context.Sale.Single(x => x.Id == id);
        }

        public void Insert(Sale entity)
        {
            _context.Sale.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<Sale> Where(Expression<Func<Sale, bool>> expression)
        {
            return _context.Sale.Where(expression).AsQueryable();
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
