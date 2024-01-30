
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductAPI.Models;
using ProductAPI.ProdDAL;

namespace ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddTransient<IEMailService, EmailService>();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ProdDBContext>(options => options.UseSqlServer
            (builder.Configuration.GetConnectionString("AppDBConnection")));

          //  builder.Services.AddDbContext<OrderDBContext>(options => options.UseSqlServer
          //(builder.Configuration.GetConnectionString("AppDBConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
