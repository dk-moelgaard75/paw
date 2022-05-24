using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeregService.Data;

namespace TimeregService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeregService", Version = "v1" });
            });
            string con = Configuration["Data:CommandApiConnectionPod:ConnectionString"];
            Console.WriteLine($"Timereg service starting with connectionstring: {con}");
            //services.AddDbContext<EmployeeDbContext>(opt => opt.UseNpgsql(con));
            if (con != null)
            {
                services.AddDbContext<TimeRegDbContext>(opt => opt.UseNpgsql(con));
            }
            else
            {
                services.AddDbContext<TimeRegDbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            }
            services.AddScoped<ITimeRegRepository, TimeRegRepository>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials()
                                            .WithExposedHeaders("Location");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeregService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //allow CORS call - according to https://www.c-sharpcorner.com/article/enable-cors-consume-web-api-by-mvc-in-net-core-4/
            //UseCors should be placed after UseRouting and before UseAuthorization
            //app.UseCors(options => options.AllowAnyOrigin());
            app.UseCors();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
