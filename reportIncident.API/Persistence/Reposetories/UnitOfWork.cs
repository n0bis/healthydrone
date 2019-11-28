using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Repositories;
using reportIncident.API.Persistence.Contexts;

namespace reportIncident.API.Persistence.Reposetories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork (AppDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
