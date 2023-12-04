using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Repositories;
using Pustok.Repositories.Implementations;
using Pustok.Services;
using Pustok.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISliderRepository,SliderRepository>();
builder.Services.AddScoped<ISliderService,SliderService>();
builder.Services.AddScoped<IBookRepository,BookRepository>();
builder.Services.AddScoped<IBookService,BookService>();


builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer("Server=MSI;Database=MVC-BB206-Crud;Trusted_Connection=True");

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
