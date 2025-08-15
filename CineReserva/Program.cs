using CineReserva.Data;
using Microsoft.EntityFrameworkCore;

namespace CineReserva
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Servicios
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // *** IMPORTANTE: servir /wwwroot (css, js, imágenes) ***
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // *** Cambiamos la ruta por defecto a Shows/Index ***
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Shows}/{action=Index}/{id?}");

            // APIs con attribute routing (BookingsController, etc.)
            app.MapControllers();

            // Semilla de datos
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DbSeeder.SeedAsync(db).GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
