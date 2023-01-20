using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Text.RegularExpressions;

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
            entityBase.District = entity.District;
            entityBase.City = entity.City;
            entityBase.Address = entity.Address;
            entityBase.Cnpj = entity.Cnpj;
            entityBase.Name = entity.Name;
            entityBase.TypeId = entity.TypeId;

            if (entity.ImageName != null) { entityBase.ImageName = entity.ImageName; }

            entityBase.Alias = GetAlias(entityBase.Name);

            _context.Entry(entityBase).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Insert(Establishment entity)
        {
            entity.Alias = GetAlias(entity.Name);
            _context.Establishment.Add(entity);
            _context.SaveChanges();
        }

        private string GetAlias(string alias)
        {
            alias = Regex.Replace(alias, "[ÄÅÁÂÀÃ]", "A");
            alias = Regex.Replace(alias, "ÉÊËÈ]", "E");
            alias = Regex.Replace(alias, "[ÍÎÏÌ]", "I");
            alias = Regex.Replace(alias, "[ÖÓÔÒÕ]", "O");
            alias = Regex.Replace(alias, "[ÜÚÛ]", "U");
            alias = Regex.Replace(alias, "[Ç]", "C");
            alias = Regex.Replace(alias, "[áàãâä]", "a");
            alias = Regex.Replace(alias, "[éèêë]", "e");
            alias = Regex.Replace(alias, "[íìîï]", "i");
            alias = Regex.Replace(alias, "[óòõôö]", "o");
            alias = Regex.Replace(alias, "[úùûü]", "u");
            alias = Regex.Replace(alias, "[ç]", "c");
            alias = Regex.Replace(alias, "[,.]", "");
            alias = Regex.Replace(alias, "[^a-zA-Z0-9 :,.]", "");
            alias = Regex.Replace(alias, "[_+/]", "");
            alias = Regex.Replace(alias, @"\s+", "");
            alias = alias.ToLower();
            return alias;
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
