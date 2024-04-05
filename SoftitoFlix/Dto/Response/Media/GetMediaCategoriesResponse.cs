using System;
namespace SoftitoFlix.Dto.Response.Media
{
	public class GetMediaCategoriesResponse
	{
		public GetMediaResponse? Media { get; set; }
        public List<GetCategoryResponse>? Categories {get; set;}
	}
}

