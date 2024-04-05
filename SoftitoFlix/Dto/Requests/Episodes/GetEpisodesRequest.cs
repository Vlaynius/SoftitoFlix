using System;
namespace SoftitoFlix.Dto.Request.Episodes
{
	public class GetEpisodesRequest
	{
		public int MediaId { get; set; }
		public byte SeasonId { get; set; }
	}
}

