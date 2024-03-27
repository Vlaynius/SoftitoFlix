using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class User_Plan
	{
		public long Id { get; set; }
        public long UserId { get; set; }
        public short PlanId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [ForeignKey("UserId")]
		public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("PlanId")]
        public Plan? Plan { get; set; }
    }
}
