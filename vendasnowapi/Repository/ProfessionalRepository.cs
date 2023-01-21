using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class ProfessionalRepository : IProfessionalRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public ProfessionalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id, string fileDelete)
        {
            var entity = _context.Professional.Single(x => x.Id == id);
            fileDelete = string.Concat(fileDelete, entity.ImageName);
            if (System.IO.File.Exists(fileDelete))
            {
                System.IO.File.Delete(fileDelete);
            }
            var services = _context.ProfessionalService.Where(c => c.ProfessionalId == id);
            _context.RemoveRange(services);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Professional Get(int id)
        {
            return _context.Professional.Single(x => x.Id == id);
        }

        public void Insert(Professional entity)
        {
            _context.Professional.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Professional entity, string fileDelete)
        {
            var entityBase = _context.Professional.Single(x => x.Id == entity.Id);
            entityBase.Name = entity.Name;
            entityBase.UpdateAspNetUsersId= entity.UpdateAspNetUsersId;
            entityBase.UpdateDate= DateTime.Now;

                fileDelete = string.Concat(fileDelete, entityBase.ImageName);
                entityBase.ImageName = entity.ImageName;
                if (System.IO.File.Exists(fileDelete))
                {
                    System.IO.File.Delete(fileDelete);
                }

            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<Professional> Where(Expression<Func<Professional, bool>> expression)
        {
            return _context.Professional.Where(expression).Include(x => x.ProfessionalService).AsQueryable();
        }

        public void Active(int id)
        {
            var entity = _context.Professional.Single(x => x.Id == id);
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
