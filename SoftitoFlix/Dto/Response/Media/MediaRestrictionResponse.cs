using System;
using SoftitoFlix.Dto.Response.Restriction;

namespace SoftitoFlix.Dto.Response.Media
{
	public class MediaRestrictionResponse
	{
		public GetMediaResponse? Media { get; set; }
		public List<GetRestrictionResponse>? Restrictions { get; set; }
	}
}

