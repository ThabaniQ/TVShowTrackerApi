namespace TVShowTracker.Contracts.Request
{
    public class EpisodeDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public bool Watched { get; set; } = false;
        public Guid ShowId { get; set; }
    }
}
