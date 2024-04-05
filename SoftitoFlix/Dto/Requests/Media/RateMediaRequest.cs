using System;
namespace SoftitoFlix.Dto.Request.Media
{
	public class RateMediaRequest
	{
		public int MediaId { get; set; }
		public byte Rating { get; set; }
	}
}

