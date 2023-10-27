using TVShowTracker.Model;

namespace TVShowTracker.Repository.Interface
{
    public interface IShowRepository : IRepositoryBase<Show>
    {
        void AddShow(Show show);

        void UpdateShow(Show show);

        void DeleteShow(Show show);

        Task<Show> GetShowAsync(Guid id,Guid userId);

        Task<IEnumerable<Show>> GetAllShowsAsync(Guid id);
    }
}
