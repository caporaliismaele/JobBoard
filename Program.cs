using JobBoard.Areas.Identity.Data;
using JobBoard.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JobBoardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging());


builder.Services.AddDefaultIdentity<JobBoardUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<JobBoardContext>();

builder.Services.AddControllersWithViews();

/*try
{
*/
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
}




    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ProfileCompletionMiddleware>();
    app.MapRazorPages();
    app.MapDefaultControllerRoute();

    await app.RunAsync();
/*}
catch (Exception ex)
{
    Console.WriteLine("Errore a runtime:");
    Console.WriteLine(ex.ToString()); // Mostra eccezione interna
    throw;
}
*/