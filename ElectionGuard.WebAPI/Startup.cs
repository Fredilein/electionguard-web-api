using ElectionGuard.Tools;
using ElectionGuard.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VotingWorks.Ballot;

namespace ElectionGuard.WebAPI
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
            services.AddControllers()
                    .AddNewtonsoftJson();
            services.AddTransient<IElectionMapper<Election, Ballot, VoteTally>, VotingWorksMapper>();
            services.AddSingleton<IConfigFileService, ConfigFileService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                        builder =>
                    {
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
            });
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

            app.UseAuthorization();

            app.UseCors("AllowAllHeaders");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
