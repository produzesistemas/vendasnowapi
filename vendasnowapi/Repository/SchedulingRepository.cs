using Models;
using System.Linq;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using Models.Filters;

namespace Repositorys
{
    public class SchedulingRepository : ISchedulingRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public SchedulingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Scheduling Get(int id)
        {
            return _context.Scheduling
                .Include(x => x.Establishment)
                .Include(x => x.SchedulingTrack).ThenInclude(x => x.StatusScheduling)
                .Include(x => x.SchedulingService).ThenInclude(x => x.Service)
                .Include(x => x.SchedulingReview)
                .Single(x => x.Id == id);
        }

        public void Insert(Scheduling entity)
        {
            entity.SchedulingEmail.Add(new SchedulingEmail()
                {
                    Send = false,
                    EmailTypeId = 1
                });
            entity.SchedulingTrack.Add(new SchedulingTrack()
                {
                    DateTrack = DateTime.Now,
                    StatusSchedulingId = 1,
                });

            _context.Scheduling.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<Scheduling> Where(Expression<Func<Scheduling, bool>> expression)
        {
            var lst = _context.Scheduling.Where(expression)
                .Include(x => x.SchedulingTrack)
                .ThenInclude(x => x.StatusScheduling)
                .Include(x => x.ApplicationUser)
                .AsQueryable();

            return lst.Select(x => new Scheduling { 
                EstablishmentId = x.EstablishmentId,
                 CreateDate = x.CreateDate,
                  Email = x.ApplicationUser.Email,
                   Id= x.Id,
                    SchedulingDate= x.SchedulingDate,
                      SchedulingTrack= x.SchedulingTrack,
                     UserName= x.ApplicationUser.UserName,                      
            }).OrderByDescending(x => x.SchedulingDate);
        }

        public IQueryable<Scheduling> GetPagination(FilterDefault filterDefault)
        {
            return _context.Scheduling
                .Where(x => x.ApplicationUserId == filterDefault.ApplicationUserId)
                .Include(x => x.Establishment).Include(x => x.SchedulingTrack)
                .ThenInclude(x => x.StatusScheduling)
                .Take(filterDefault.SizePage).AsQueryable();
        }

        public void CancelByClient(int id)
        {
            _context.SchedulingEmail.Add(new SchedulingEmail()
            {
                Send = false,
                EmailTypeId = 2,
                SchedulingId = id
            });
            _context.SchedulingTrack.Add(new SchedulingTrack()
            {
                DateTrack = DateTime.Now,
                StatusSchedulingId = 2,
                SchedulingId = id
            });
            _context.SaveChanges();
        }

        public void CancelByPartner(int id)
        {
            _context.SchedulingEmail.Add(new SchedulingEmail()
            {
                Send = false,
                EmailTypeId = 3,
                SchedulingId = id
            });
            _context.SchedulingTrack.Add(new SchedulingTrack()
            {
                DateTrack = DateTime.Now,
                StatusSchedulingId = 3,
                SchedulingId = id
            });
            _context.SaveChanges();
        }

        public void Confirm(int id)
        {
            _context.SchedulingEmail.Add(new SchedulingEmail()
            {
                Send = false,
                EmailTypeId = 4,
                SchedulingId = id
            });
            _context.SchedulingTrack.Add(new SchedulingTrack()
            {
                DateTrack = DateTime.Now,
                StatusSchedulingId = 4,
                SchedulingId = id
            });
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
