using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Dtos
{
	public class ApplicationUser_Dto
	{

		public string userName { get; set; } = "";
		[EmailAddress]
		public string email { get; set; } = "";
		[Phone]
		public string phoneNumber { get; set; } = "";

		public DateTime BirthDate { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string name { get; set; } = "";

		[NotMapped]
		public string? password { get; set; } 
	}
}

