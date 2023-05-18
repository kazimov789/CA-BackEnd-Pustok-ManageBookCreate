using Microsoft.EntityFrameworkCore;
using P328Pustok.DAL;
using P328Pustok.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PustokContext>(opt =>
{
    opt.UseSqlServer("Server=LAPTOP-DMGD9EDH\\SQLEXPRESS;Database=PustokV2-Home;Trusted_Connection=True");
});

builder.Services.AddSession();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
