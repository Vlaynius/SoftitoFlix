using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftitoFlix.Data;
using SoftitoFlix.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SoftitoFlix;

public class Program
{
    public static void Main(string[] args)
    {
        Restriction restriction;
        ApplicationUser applicationUser;
        IdentityRole identityRole;
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDatabase")));
        builder.Services.AddDefaultIdentity<ApplicationUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();//AddDefaultTokenProviders()
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        ApplicationDbContext? context = app.Services.CreateScope().ServiceProvider.GetService<ApplicationDbContext>();
        if (context != null)
        {
            context.Database.Migrate();
            if(context.Restrictions.Count() == 0 )
            {
                restriction = new Restriction();
                restriction.Name = "Genel Izleyici";
                restriction.Id = 0;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "18";
                restriction.Id = 18;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "13";
                restriction.Id = 13;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "7";
                restriction.Id = 7;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "Korku ve Şiddet";
                restriction.Id = 19;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "Olumsuz Örnek";
                restriction.Id = 20;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);


            }
            context.SaveChanges();
            RoleManager<IdentityRole>? roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager != null)
            {
                if (roleManager.Roles.Count() == 0)
                {
                    identityRole = new IdentityRole("Administrator");
                    roleManager.CreateAsync(identityRole).Wait();
                    identityRole = new IdentityRole("ContentAdmin");
                    roleManager.CreateAsync(identityRole).Wait();
                    identityRole = new IdentityRole("CustomerRepresentative");
                    roleManager.CreateAsync(identityRole).Wait();
                }
            }


            UserManager<ApplicationUser>? userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
            if (userManager != null)
            {
                if (userManager.Users.Count() == 0)
                {
                    applicationUser = new ApplicationUser();
                    applicationUser.UserName = "Administrator";
                    applicationUser.Name = "Administrator";
                    applicationUser.Email = "abc@def.com";
                    applicationUser.PhoneNumber = "1112223344";
                    applicationUser.BirthDate = DateTime.Today;
                    applicationUser.Passive = false;
                    applicationUser.Deleted = false;
                    userManager.CreateAsync(applicationUser, "Admin123!").Wait();
                    userManager.AddToRoleAsync(applicationUser, "Administrator").Wait();
                }
            }

        }
            app.Run();
    }
}
