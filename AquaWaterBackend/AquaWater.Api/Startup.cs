using AquaWater.Data.Context;
using AquaWater.Data.Extension;
using AquaWater.Data.Seed;
using AquaWater.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AquaWater.Data.Configuration;
using AquaWater.Dto.Automapper;
using AquaWater.Domain.Settings;
using Mailjet.Client;

namespace AquaWater.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureServices();
            services.AddDbContextAndConfigurations(Environment, Configuration);
            services.ConfigurationIdentity();
            //services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<ImageUploadSettings>(Configuration.GetSection("ImageUploadSettings"));
            services.AddControllers();
            services.ConfigureAuthentication(Configuration);
            services.AddSwaggerConfiguration();
            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
            {
                client.UseBasicAuthentication(Configuration.GetSection("MailJetKeys")["ApiKey"], Configuration.GetSection("MailJetKeys")["ApiSecret"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AquaWater.Api v1"));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            Seeder.Seed(roleManager, userManager, dbContext).GetAwaiter().GetResult();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
