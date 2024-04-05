using System;
using SoftitoFlix.Models;

namespace SoftitoFlix.Dto.Response
{
	public class GetCategoryMediasResponse
	{
		public Category? Category { get; set; }
		public List<GetMediaResponse>? medias { get; set; }
	}
}

