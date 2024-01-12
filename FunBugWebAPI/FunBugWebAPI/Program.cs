using FunBugWebAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace FunBugWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string provider = builder.Configuration.GetValue("Provider", "SqlServer");
            builder.Services.AddDbContext<ApplicationDbContext>
            (
                options => _ = provider switch
                {
                    "SqlServer" => options.UseSqlServer
                    (
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("SqlServerLib")
                    ),
                    "MySql" => options.UseMySQL
                    (
                         builder.Configuration.GetConnectionString("MySqlConnection"),
                         x => x.MigrationsAssembly("MySqlLib")
                    ),

                    _ => throw new Exception($"Provider {provider} not supported.")
                }


            );

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();

            app.Run();
        }
    }
}