using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace UnitOfWork
{
    public interface IProductRepository : IDisposable
    {
        Product Get(int id);
        IQueryable<Product> Where(Expression<Func<Product, bool>> expression);
        void Delete(int id);
        void Update(Product entity);
        void Insert(Product entity);
        IQueryable<Product> GetPagination(Expression<Func<Product, bool>> expression, int sizePage);
    }
}
