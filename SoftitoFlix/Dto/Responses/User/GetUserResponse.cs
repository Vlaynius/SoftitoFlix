using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftitoFlix.Dto.Responses.User
{
    public class GetUserResponse
    {
        public DateTime BirthDate { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = "";

        [EmailAddress]
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Email { get; set; } = "";

        [Phone]
        [Column(TypeName = "nchar(100)")]
        [StringLength(100, MinimumLength = 10)]
        public string Phone { get; set; } = "";
    }
}
