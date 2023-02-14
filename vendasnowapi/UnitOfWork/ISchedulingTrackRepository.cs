using Models;
using System;
namespace UnitOfWork
{
    public interface ISchedulingTrackRepository : IDisposable
    {
        void Insert(SchedulingTrack entity);
    }
}
