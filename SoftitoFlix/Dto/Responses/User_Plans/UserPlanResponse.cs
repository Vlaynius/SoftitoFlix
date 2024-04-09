using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Dto.Responses.User_Plans
{
    public class UserPlanResponse
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;

        public string PlanName { get; set; } = string.Empty;
        public float PlanCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
