using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class User_Favorite
	{
        public long UserId { get; set; }
        public int MediaId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("MediaId")]
        public Media? Media { get; set; }
    }
}
