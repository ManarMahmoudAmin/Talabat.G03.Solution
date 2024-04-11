
using Microsoft.EntityFrameworkCore;
using Talabat.Infrastructure.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args); //the builder that will create our web application
            /*When we create the webApplicationBuilder --> start to configure 
			 *We Configure the webApplicationBuilder with 7 things 
		   	 *The Srvices is one of them*/

            #region Configure Service
            // Add services to the container. Register the services to the Dependency Injection Container

            webApplicationBuilder.Services.AddControllers();// Register Services requierd by APIs

            #region Register Swagger Services in the DI Container
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            #endregion


            webApplicationBuilder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            var app = webApplicationBuilder.Build();  //the Kestrel 

            #region Configure Kestrel Middelware
            // Configure the HTTP request pipeline. // determine the middleware of the app // NOTE: the middleware must be in order
            if (app.Environment.IsDevelopment())
            {//Document our API // no need to Document API in Production phase as it will be deployed on server and consumed by the frontend/mobile developer
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers(); //reads the route of the controller from the controller Attribute Decorator
            #endregion

            app.Run();
        }
    }
}
