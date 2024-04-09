using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftitoFlix.Dto.Requests.User
{
    public class LogInModel
    {
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string userName { get; set; } = string.Empty;
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, MinimumLength = 8)]
        public string password { get; set; } = string.Empty;
    }
}
