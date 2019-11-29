using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Persistence.Contexts;

namespace reportIncident.API.Persistence.Reposetories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;


        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
