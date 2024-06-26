﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
namespace SoftitoFlix.Models
{
	public class Restriction
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public byte Id { get; set; }
		[Column(TypeName = "nvarchar(100)")]
		[StringLength(100, MinimumLength = 2)]
		[Required]
		public string Name { get; set; } = "";
		public bool Passive { get; set; }
    }
}
