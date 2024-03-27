using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class User_Watched
	{
        public long UserId { get; set; }
        public long EpisodeId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("EpisodeId")]
        public Episode? Episode { get; set; }
    }
}
