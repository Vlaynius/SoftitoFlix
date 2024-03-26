using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Episode
	{
		public int MediaId { get; set; }
		public byte SeasonNumber { get; set; }
		public int EpisodeNumber { get; set; }
		public DateTime ReleaseDate { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public required string Title { get; set; }
        [Column(TypeName = "nvarchar(450)")]
        [StringLength(450)]
        public string? Description { get; set; }
        public int Duration { get; set; }
        [ForeignKey("MediaId")]
        public Media? Media { get; set; }

    }
}
