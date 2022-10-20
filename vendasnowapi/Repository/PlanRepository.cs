using Models;
using System.Linq;
using UnitOfWork;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class PlanRepository : IPlanRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public PlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Plan Get(int id)
        {
            return _context.Plan.Single(x => x.Id == id);
        }

        public IQueryable<Plan> Where(Expression<Func<Plan, bool>> expression)
        {
            return _context.Plan.Where(expression).AsQueryable();
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
