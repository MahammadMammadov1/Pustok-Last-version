using Microsoft.EntityFrameworkCore;
using Pustok.DAL;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer("Server=DESKTOP-KA8SSD4;Database=MVC-BB206-Crud-FILE;Trusted_Connection=True");

});



var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
