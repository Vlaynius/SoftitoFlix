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
        public required string Name { get; set; }
        [Column(TypeName = "nvarchar(450)")]
        [StringLength(450)]
        public required string Description { get; set; }
        public List<Media_Category>? Media_Categories { get; set; }
        public List<Media_Director>? Media_Directors { get; set; }
        public List<Media_Star>? Media_Stars { get; set; }
        public List<Media_Restriction>? Media_Restrictions { get; set; }
        [Range(0,10)]
        public float Rating { get; set; }
        public bool Passive { get; set; } 
    }
}
