using System;

namespace BabyBus.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        BabyBusDataContext Get();
    }
}
