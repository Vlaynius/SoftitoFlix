using System;
using Microsoft.EntityFrameworkCore;

namespace SoftitoFlix.Data
{
	public class ApplicationDbContext 
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {

        }
       
	}
}

