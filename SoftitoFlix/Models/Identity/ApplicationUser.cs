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
        public string Name { get; set; } = "";

        [NotMapped]
        [StringLength(100,MinimumLength = 8)]
        public string? Password { get; set; }
        public bool Passive { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public byte? Restriction
        {
            get
            {
                DateTime date = BirthDate.AddYears(7);
                if(date > DateTime.Today)
                {
                    return 7; 
                }
                date = BirthDate.AddYears(6);
                if (date > DateTime.Today)
                {
                    return 13;
                }
                date = BirthDate.AddYears(5);
                if (date > DateTime.Today)
                {
                    return 18;
                }
                return null;
            }
        }
    }
}
