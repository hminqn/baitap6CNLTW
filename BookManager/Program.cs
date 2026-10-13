using BookManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.DataProtection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, ".keys")));
var provider=builder.Configuration["DatabaseProvider"] ?? "Sqlite";
builder.Services.AddDbContext<LibraryContext>(options => {
    if(provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    else options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});
var app=builder.Build();
using(var scope=app.Services.CreateScope()) {
    var db=scope.ServiceProvider.GetRequiredService<LibraryContext>();
    if(provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)) db.Database.EnsureCreated();
    else db.Database.Migrate();
}
var culture=new CultureInfo("vi-VN");
app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture("vi-VN").AddSupportedCultures("vi-VN").AddSupportedUICultures("vi-VN"));
if(!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name:"default", pattern:"{controller=Books}/{action=Index}/{id?}");
app.Run();
