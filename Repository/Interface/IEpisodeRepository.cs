using TVShowTracker.Model;

namespace TVShowTracker.Repository.Interface
{
    public interface IEpisodeRepository : IRepositoryBase<Episode>
    {
        void AddEpisode(Episode episode);

        void UpdateEpisode(Episode episode);

        void DeleteEpisode(Episode episode);

        Task<Episode> GetEpisodeAsync(Guid id,Guid showId);

        Task<IEnumerable<Episode>> GetAllEpisodesAsync(Guid showId);

    }
}
