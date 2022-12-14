using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using LinqKit;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Repositorys
{
    public class EstablishmentRepository : IEstablishmentRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public EstablishmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Establishment Get(int id)
        {
            return _context.Establishment
                .Single(x => x.Id == id);
        }

        public IQueryable<Establishment> Where(Expression<Func<Establishment, bool>> expression)
        {
           return _context.Establishment.Where(expression)
                .AsQueryable();
        }

        public IQueryable<Establishment> GetAll()
        {
            return _context.Establishment.AsQueryable();
        }

        public void Active(int id)
        {
            var entity = _context.Establishment.Single(x => x.Id == id);
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

        public void Delete(int id)
        {
            var entity = _context.Establishment.Single(x => x.Id == id);
            //var _establishmentPartnerTypes = _context.EstablishmentPartnerType.Where(c => c.EstablishmentId == id);
            //_context.RemoveRange(_establishmentPartnerTypes);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Establishment entity)
        {
            var entityBase = _context.Establishment.Single(x => x.Id == entity.Id);

            entityBase.Description = entity.Description;
            entityBase.Scheduling = entity.Scheduling;

            if (entity.ImageName != null) { entityBase.ImageName = entity.ImageName; }

            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Insert(Establishment entity)
        {
            _context.Establishment.Add(entity);
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

        public Establishment GetByUser(string id)
        {
            Expression<Func<Establishment, bool>> pred1;
            var pred = PredicateBuilder.New<Establishment>();
            pred1 = p => p.AspNetUsersId.Equals(id);
            pred = pred.And(pred1);

            var establishment = _context.Establishment.FirstOrDefault(pred);

            if (establishment != null)
            {
                Expression<Func<Subscription, bool>> ps1, ps2;
                var predicate = PredicateBuilder.New<Subscription>();
                ps1 = p => p.AspNetUsersId.Equals(id);
                predicate = predicate.And(ps1);
                ps2 = p => p.Active == true;
                predicate = predicate.And(ps2);
                establishment.Subscription = _context.Subscription
                    .Include(x => x.Plan)
                    .Where(predicate)
                    .OrderByDescending(x => x.SubscriptionDate)
                    .FirstOrDefault();
            }

            return establishment;
        }
    }
}
