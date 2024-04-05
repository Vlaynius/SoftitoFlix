using System;
namespace SoftitoFlix.Dto.Response.Directors
{
	public class GetDirectorMediasResponse
	{
		
		public GetDirectorResponse? Director { get; set; }
		public List<GetMediaResponse>? Medias { get; set; }
	}
}

