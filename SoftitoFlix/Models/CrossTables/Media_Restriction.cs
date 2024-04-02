using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Media_Restriction
	{
        public int MediaId { get; set; }
        public byte RestrictionId { get; set; }
        [ForeignKey("MediaId")]
        public Media? Media { get; set; }
        [ForeignKey("RestrictionId")]
        public Restriction? Restriction { get; set; }
    }
}

