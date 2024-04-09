using System.ComponentModel.DataAnnotations;

namespace SoftitoFlix.Dto.Requests.User_Plans
{
    public class Plan_Purchase
    {
        [EmailAddress]
        public string eMail { get; set; } = string.Empty;
        public short planId { get; set; }
    }
}
