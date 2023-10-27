using System.ComponentModel.DataAnnotations;

namespace TVShowTracker.Contracts.Request
{
    public class ShowDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
