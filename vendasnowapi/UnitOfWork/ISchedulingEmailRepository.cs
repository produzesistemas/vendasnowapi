using Models;
using System;
namespace UnitOfWork
{
    public interface ISchedulingEmailRepository : IDisposable
    {
        void Insert(SchedulingEmail entity);
    }
}
