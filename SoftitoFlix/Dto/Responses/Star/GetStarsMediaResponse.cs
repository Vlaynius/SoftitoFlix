using SoftitoFlix.Dto.Response;

namespace SoftitoFlix.Dto.Responses.Star
{
    public class GetStarsMediaResponse
    {
        public GetStarResponse? Star { get; set; }
        public List<int>? MediaIDs { get; set; }
    }
}
