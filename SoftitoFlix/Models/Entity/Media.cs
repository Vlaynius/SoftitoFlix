using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Media
	{
		public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = "";
        [Column(TypeName = "nvarchar(450)")]
        [StringLength(450)]
        public string? Description { get; set; }
        public virtual List<Media_Category>? Media_Categories { get; set; }
        public virtual List<Media_Director>? Media_Directors { get; set; }
        public virtual List<Media_Star>? Media_Stars { get; set; }
        public virtual List<Media_Restriction>? Media_Restrictions { get; set; }
        [Range(1800,short.MaxValue)]
        public short ReleaseDate { get; set; }
        public bool Passive { get; set; }
        [Range(0, 10)]
        public float? Rating { get; set; }
        [Range(0,long.MaxValue)]
        public long RatedBy { get; set; }
    }
}
