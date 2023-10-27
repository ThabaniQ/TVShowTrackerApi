using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TVShowTracker.Data;
using TVShowTracker.Model;
using TVShowTracker.Repository.Interface;

namespace TVShowTracker.Repository
{
    public class ShowRepository : RepositoryBase<Show>, IShowRepository
    {
        public ShowRepository(ApplicationDbContext applicationContext) : base(applicationContext) { }

        public void AddShow(Show show)
        {
            CreateAsync(show);
        }

        public void DeleteShow(Show show)
        {
            DeleteAsync(show);
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync(Guid userId)
        {
            return await FindByConditionAsync(x => x.UserId.Equals(userId.ToString()))
                .OrderBy(x => x.Title)
                .ToListAsync();
        }

        public async Task<Show> GetShowAsync(Guid id, Guid userId)
        {
            return await FindByConditionAsync(x => x.Id.Equals(id) && x.UserId.Equals(userId.ToString()))
                .FirstOrDefaultAsync();
        }

        public void UpdateShow(Show show)
        {
            UpdateAsync(show);
        }
    }
}
