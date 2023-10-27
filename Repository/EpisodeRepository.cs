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
    public class EpisodeRepository : RepositoryBase<Episode>, IEpisodeRepository
    {
        public EpisodeRepository(ApplicationDbContext applicationContext) : base(applicationContext) { }

        public void AddEpisode(Episode episode)
        {
            CreateAsync(episode);
        }

        public void DeleteEpisode(Episode episode)
        {
            DeleteAsync(episode);
        }

        public async Task<Episode> GetEpisodeAsync(Guid id, Guid showId)
        {
            return await FindByConditionAsync(x => x.Id.Equals(id) && x.ShowId.Equals(showId))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Episode>> GetAllEpisodesAsync(Guid showId)
        {
            return await FindByConditionAsync(x => x.ShowId.Equals(showId))
                .OrderBy(x => x.Title)
                .ToListAsync();
        }

        public void UpdateEpisode(Episode episode)
        {
            UpdateAsync(episode);
        }
    }
}
