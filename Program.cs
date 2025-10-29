using SimpleQuizApp.Data;
using SimpleQuizApp.Services;

namespace SimpleQuizApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            /// <summary>
            /// Registers the <see cref="MemoryStore"/> class as a singleton service in the 
            /// dependency injection container.
            /// This ensures that a single shared instance of <see cref="MemoryStore"/> 
            /// is used throughout the application lifetime, maintaining consistent 
            /// in-memory data across requests
            /// </summary>
            builder.Services.AddSingleton<MemoryStore>();

            var app = builder.Build();

            /// <summary>
            /// Calls the <see cref="AppDbInitializer.Seed(IApplicationBuilder)"/> method 
            /// to populate the application's data store with initial data
            /// </summary>
            AppDbInitializer.Seed(app);

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
