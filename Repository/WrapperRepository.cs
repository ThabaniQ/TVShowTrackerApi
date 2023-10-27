using System.Threading.Tasks;
using TVShowTracker.Data;
using TVShowTracker.Repository.Interface;

namespace TVShowTracker.Repository
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly ApplicationDbContext _context;
        private IShowRepository _showRepository;
        private IEpisodeRepository _episodeRepository;
        private IIdentityService _identityService;

        public WrapperRepository(ApplicationDbContext context, IShowRepository showRepository, IEpisodeRepository episodeRepository, IIdentityService identityService)
        {
            _context = context;
            _showRepository = showRepository;
            _episodeRepository = episodeRepository;
            _identityService = identityService;
        }

        public IShowRepository Show => _showRepository;
        public IEpisodeRepository Episode => _episodeRepository;

        public IIdentityService Identity => _identityService;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
