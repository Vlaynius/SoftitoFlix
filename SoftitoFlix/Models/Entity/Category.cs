﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftitoFlix.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		[StringLength(50, MinimumLength = 2)]
		public string Name { get; set; } = "";
    }
}
