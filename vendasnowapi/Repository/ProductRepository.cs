using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var entity = _context.Product.Single(x => x.Id == id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Product Get(int id)
        {
            return _context.Product.Single(x => x.Id == id);
        }

        public void Insert(Product entity)
        {
            _context.Product.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Product entity)
        {
            var entityBase = _context.Product.Single(x => x.Id == entity.Id);
            entityBase.Name = entity.Name;
            entityBase.Value = entity.Value;
            if (entity.CostValue.HasValue) { entityBase.CostValue = entity.CostValue.Value; }
            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _context.Product.Where(expression).AsQueryable();
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

        public IQueryable<Product> GetPagination(Expression<Func<Product, bool>> expression, int sizePage)
        {
            return _context.Product.Where(expression).Take(sizePage);
        }
    }
}
