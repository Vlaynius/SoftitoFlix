using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SoftitoFlix.Models
{
	public class ApplicationUser : IdentityUser<long>
	{
		[Column(TypeName = "date")]
		public DateTime BirthDate { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public required string Name { get; set; }
        [NotMapped]
        [StringLength(100,MinimumLength = 8)]
        public required string Password { get; set; }
        public bool Passive { get; set; } 
    }
}
