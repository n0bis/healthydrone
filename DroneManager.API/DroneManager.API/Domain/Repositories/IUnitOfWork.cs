using System;
using System.Threading.Tasks;

namespace DroneManager.API.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
