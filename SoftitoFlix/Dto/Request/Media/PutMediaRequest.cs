using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Dto.Request.Media
{
	public class PutMediaRequest
	{
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = "";
        [Column(TypeName = "nvarchar(450)")]
        [StringLength(450)]
        public string? Description { get; set; }
        [Range(1800, short.MaxValue)]
        public short ReleaseDate { get; set; }
    }
}

