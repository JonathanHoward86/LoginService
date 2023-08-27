using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LoginService.Models;

namespace LoginService
{
    public class Startup
    {
        // Constructor to initialize configuration
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Configuration property
        public IConfiguration Configuration { get; }

        // Method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext and configure connection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add MVC controllers and views
            services.AddControllersWithViews();

            // Add EmailService as a transient service
            services.AddTransient<IEmailService, EmailService>();

            // Add Identity services and configure options
            services.AddIdentity<IdentityUser, IdentityRole>(options => { })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
        }

        // Method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable routing
            app.UseRouting();

            // Enable authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure endpoint routing
            app.UseEndpoints(endpoints =>
            {
                // Default route configuration
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");

                // Map other controllers
                endpoints.MapControllers();
            });
        }
    }
}