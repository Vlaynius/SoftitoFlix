using System;
using Microsoft.AspNetCore.Identity;

namespace SoftitoFlix.Models
{
	public class ApplicationRole : IdentityRole<long>
	{
		public ApplicationRole(string Name) : base(Name)
		{
           
        }

        public ApplicationRole()
        {
            
        }
    }
}

