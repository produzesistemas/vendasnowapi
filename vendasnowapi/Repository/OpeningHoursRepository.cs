using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class OpeningHoursRepository : IOpeningHoursRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public OpeningHoursRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var entity = _context.OpeningHours.Single(x => x.Id == id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public OpeningHours Get(int id)
        {
            return _context.OpeningHours.Single(x => x.Id == id);
        }

        public void Insert(OpeningHours entity)
        {
            _context.OpeningHours.Add(entity);
            _context.SaveChanges();
        }

        public void Update(OpeningHours entity)
        {
            var entityBase = _context.OpeningHours.Single(x => x.Id == entity.Id);
            entityBase.Weekday = entity.Weekday;
            entityBase.StartTime = entity.StartTime;
            entityBase.EndTime = entity.EndTime;
            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<OpeningHours> Where(Expression<Func<OpeningHours, bool>> expression)
        {
            return _context.OpeningHours.Where(expression).AsQueryable();
        }


        public void Active(int id)
        {
            var entity = _context.OpeningHours.Single(x => x.Id == id);
            if (entity.Active)
            {
                entity.Active = false;
            }
            else
            {
                entity.Active = true;
            }
            _context.Entry(entity).State = EntityState.Modified;
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
