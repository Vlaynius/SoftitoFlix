using System;
using SoftitoFlix.Dto.Response.Directors;

namespace SoftitoFlix.Dto.Response.Media
{
	public class MediaDirectorsResponse
	{
		public List<GetMediaResponse>? Medias { get; set; }
		public GetDirectorResponse? Director { get; set; } 
	}
}

