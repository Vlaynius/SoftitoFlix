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
        ApplicationRole identityRole;
        Category category;
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDatabase")));

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        

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
            if(!context.Restrictions.Any())
            {
                restriction = new Restriction();
                restriction.Name = "Genel Izleyici";
                restriction.Id = 0;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "7";
                restriction.Id = 7;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "13";
                restriction.Id = 13;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);
                restriction = new Restriction();
                restriction.Name = "18";
                restriction.Id = 18;
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
                restriction = new Restriction();
                restriction.Name = "Cinsellik";
                restriction.Id = 21;
                restriction.Passive = false;
                context.Restrictions.Add(restriction);

            }

            if (!context.Categories.Any())
            {
                category = new Category();
                category.Name = "Bilim-Kurgu";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Aksiyon";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Gerilim";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Korku";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Komedi";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Romantik";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Animasyon";
                context.Categories.Add(category);
                category = new Category();
                category.Name = "Fantastik";
                context.Categories.Add(category);
            }

            context.SaveChanges();
            RoleManager<ApplicationRole>? roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<ApplicationRole>>();
            if (roleManager != null)
            {
                if (!roleManager.Roles.Any())
                {
                    identityRole = new ApplicationRole("Administrator");
                    roleManager.CreateAsync(identityRole).Wait();
                    identityRole = new ApplicationRole("ContentAdmin ");
                    roleManager.CreateAsync(identityRole).Wait();
                    identityRole = new ApplicationRole("CustomerRepresentative");
                    roleManager.CreateAsync(identityRole).Wait();
                }
            }

            UserManager<ApplicationUser>? userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
            if (userManager != null)
            {
                if (!userManager.Users.Any())
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

                    applicationUser = new ApplicationUser();
                    applicationUser.UserName = "ContentAdmin";
                    applicationUser.Name = "ContentAdmin";
                    applicationUser.Email = "abc@def.com";
                    applicationUser.PhoneNumber = "1112223344";
                    applicationUser.BirthDate = DateTime.Today;
                    applicationUser.Passive = false;
                    applicationUser.Deleted = false;
                    userManager.CreateAsync(applicationUser, "Admin123!").Wait();
                    userManager.AddToRoleAsync(applicationUser, "ContentAdmin").Wait();

                    applicationUser = new ApplicationUser();
                    applicationUser.UserName = "CustomerRepresentative";
                    applicationUser.Name = "CustomerRepresentative";
                    applicationUser.Email = "abc@def.com";
                    applicationUser.PhoneNumber = "1112223344";
                    applicationUser.BirthDate = DateTime.Today;
                    applicationUser.Passive = false;
                    applicationUser.Deleted = false;
                    userManager.CreateAsync(applicationUser, "Admin123!").Wait();
                    userManager.AddToRoleAsync(applicationUser, "CustomerRepresentative").Wait();
                }

            }

        }



        app.Run();
    }
}
