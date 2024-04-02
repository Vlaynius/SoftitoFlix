using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Media_Category
	{
		public int MediaId { get; set; }
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category? Category { get; set; }
		[ForeignKey("MediaId")]
		public Media? Media { get; set; }
    }
}

