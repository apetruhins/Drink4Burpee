using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Drink4Burpee.Services;
using Drink4Burpee.Services.Interfaces;
using AutoMapper;
using System.Reflection;
using Drink4Burpee.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;

namespace Drink4Burpee
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
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetValue<string>("GuiBaseURL"))
                            .WithMethods("GET", "POST")
                            .WithHeaders("Content-Type");
                    });
            });

            services.Configure<Drink4BurpeeDbSettings>(Configuration.GetSection(nameof(Drink4BurpeeDbSettings)));

            services.AddControllers();

            services.AddSingleton<IDrink4BurpeeDbSettings>(sp => sp.GetRequiredService<IOptions<Drink4BurpeeDbSettings>>().Value);

            services.AddSingleton<IUserService, UserService>();
            
            services.AddTransient<IDrinkService, DrinkService>();
            services.AddTransient<IDrinkBurpeeService, DrinkBurpeeService>();
            services.AddTransient<IExerciseBurpeeService, ExerciseBurpeeService>();

            var enumConvention = new ConventionPack { new EnumRepresentationConvention(BsonType.String) };
            ConventionRegistry.Register("enumConvention", enumConvention, t => true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
