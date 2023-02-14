using Models;
using UnitOfWork;
using System;
namespace Repositorys
{
    public class SchedulingTrackRepository : ISchedulingTrackRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool disposed = false;

        public SchedulingTrackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Insert(SchedulingTrack entity)
        {
            _context.SchedulingTrack.Add(entity);
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
