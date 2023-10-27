using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace TVShowTracker.Model
{
    public class Episode
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        [Required]
        public int SeasonNumber { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [Required]
        public bool Watched { get; set; }

        
        public Guid ShowId { get; set; }

        [ForeignKey(nameof(ShowId))]
        public Show Show { get; set; }
    }
}



