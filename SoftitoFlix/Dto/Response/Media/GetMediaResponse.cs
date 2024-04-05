using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Dto.Response
{
	public class GetMediaResponse
	{
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = "";
        [Column(TypeName = "nvarchar(450)")]
        [StringLength(450)]
        public string? Description { get; set; }
        [Range(1800, short.MaxValue)]
        public short ReleaseDate { get; set; }
        [Range(0, 10)]
        public float Rating { get; set; }
    }
}

