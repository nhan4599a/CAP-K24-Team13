using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IEFCoreRepository<T> : IDisposable where T : DbContext
    {
        T DbContext { get; }
    }
}
