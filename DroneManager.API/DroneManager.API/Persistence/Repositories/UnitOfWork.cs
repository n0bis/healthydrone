using System;
using System.Threading.Tasks;
using DroneManager.API.Domain.Repositories;
using DroneManager.API.Persistence.Contexts;

namespace DroneManager.API.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
