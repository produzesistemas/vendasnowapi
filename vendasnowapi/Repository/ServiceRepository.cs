using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
namespace Repositorys
{
    public class ServiceRepository : IServiceRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var entity = _context.Service.Single(x => x.Id == id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public Service Get(int id)
        {
            return _context.Service.Single(x => x.Id == id);
        }

        public void Insert(Service entity)
        {
            _context.Service.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Service entity, string fileDelete)
        {
            var entityBase = _context.Service.Single(x => x.Id == entity.Id);
            entityBase.Description = entity.Description;
            entityBase.UpdateAspNetUsersId= entity.UpdateAspNetUsersId;
            entityBase.UpdateDate= DateTime.Now;
            entityBase.Value = entity.Value;
            if (entity.Minute.HasValue) { entityBase.Minute = entity.Minute.Value; }

                fileDelete = string.Concat(fileDelete, entityBase.ImageName);
                entityBase.ImageName = entity.ImageName;
                if (System.IO.File.Exists(fileDelete))
                {
                    System.IO.File.Delete(fileDelete);
                }

            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IQueryable<Service> Where(Expression<Func<Service, bool>> expression)
        {
            return _context.Service.Where(expression).AsQueryable();
        }

        public void Active(int id)
        {
            var entity = _context.Service.Single(x => x.Id == id);
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
