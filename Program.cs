using Microsoft.EntityFrameworkCore;
using SimpleQuizApp.Data;

namespace SimpleQuizApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            var connectionString = builder.Configuration.GetConnectionString("TestDb")!;
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(connectionString));

            var app = builder.Build();
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            AppDbInitializer.Seed(app);

            app.Run();
        }
    }
}
