using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftitoFlix.Dto.Requests.User
{
    public class ChangePassword
    {
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string UserName { get; set; }  = string.Empty;
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, MinimumLength = 8)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
