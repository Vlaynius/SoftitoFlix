using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Dto.Requests.Star
{
	public class PostStarRequest
	{
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = "";
    }
}

