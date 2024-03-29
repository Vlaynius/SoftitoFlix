using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
namespace SoftitoFlix.Models
{
	public class Restriction
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public byte Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
		[Required]
        public required string Name { get; set; }
		public bool Passive { get; set; }
    }
}

/*
 id = 0 --> 7+
 id = 1 --> 13+
 id = 2 --> 18+
 
 */