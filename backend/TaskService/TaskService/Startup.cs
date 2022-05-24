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
using TaskService.AsyncDataServices;
using TaskService.Data;
using TaskService.Utils;

namespace TaskService
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
            PawLogger.DoLog("Task service start:" + DateTime.Now);
            
            //RabbitMQ
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            
            services.AddControllers();
            services.AddHostedService<MessageBusSubscriber>();
            /*
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskService", Version = "v1" });
            });
            */
            string con = Configuration["Data:CommandApiConnectionPod:ConnectionString"];
            Console.WriteLine($"Task service starting with connectionstring: {con}");
            if (con != null)
            {
                services.AddDbContext<TaskObjDbContext>(opt => opt.UseNpgsql(con));
            }
            else
            {
                services.AddDbContext<TaskObjDbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DbConnection")));
            }

            services.AddScoped<ITaskObjRepository, TaskObjRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                /*
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskService v1"));
                */
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
