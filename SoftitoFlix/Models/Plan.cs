using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Plan
	{
        [Key]
		public short Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }
		[Range(0,float.MaxValue)]
		public float Price { get; set; }
        [Column(TypeName = "varchar(20)")]
        [StringLength(20, MinimumLength = 2)]
        public required string Resolution { get; set; }
        public bool Passive { get; set; }
    }
}

