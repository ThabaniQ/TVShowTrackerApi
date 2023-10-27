using System;
using System.Threading.Tasks;

namespace TVShowTracker.Repository.Interface
{
    public interface IWrapperRepository
    {
        IShowRepository Show { get; }

        IEpisodeRepository Episode { get; }

        IIdentityService Identity { get; }

        Task SaveAsync();
    }
}
