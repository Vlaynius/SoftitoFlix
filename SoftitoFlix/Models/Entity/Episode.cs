using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Episode
	{
        [Key]
        public long Id { get; set; }
		public int MediaId { get; set; }
        [Range(0,byte.MaxValue)]
		public byte SeasonNumber { get; set; }
        [Range(0,366)]
		public short EpisodeNumber { get; set; }
		public DateTime ReleaseDate { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = "";
        [Column(TypeName = "nvarchar(450)")]
        [StringLength(450)]
        public string? Description { get; set; }
        public TimeSpan Duration { get; set; }
        public long ViewCount { get; set; }
        public bool Passive { get; set; } 
        [ForeignKey("MediaId")]
        public Media? Media { get; set; }

    }
}
