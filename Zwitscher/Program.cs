using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zwitscher.Components;
using Zwitscher.Data;
using Zwitscher.Models;
using Zwitscher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<UserService>();

// 1) DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;   // falls du keine E-Mail erzwingst
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// in Program.cs, nach builder.Services.AddIdentity<...>()
builder.Services.ConfigureApplicationCookie(options =>
{
    // Bei fehlender Authentifizierung hierhin umleiten:
    options.LoginPath = "/login";
    // Optional: wenn keine Berechtigung:
    options.AccessDeniedPath = "/";
});


// 3) MVC / RazorPages / Blazor-Authentication
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication();     // Identity selbst registriert bereits Cookie-Auth
builder.Services.AddAuthorization();

async Task SeedRolesAndAdminAsync(IServiceProvider services)
{
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = services.GetRequiredService<UserManager<User>>();

    // 1) Rollen anlegen
    foreach (var role in Enum.GetNames(typeof(Roles)))
    {
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new IdentityRole(role));
    }

    // 2) Admin-Benutzer anlegen
    string adminName = "admin";
    if (await userMgr.FindByNameAsync(adminName) == null)
    {
        var admin = new User
        {
            UserName = adminName,
            CreatedAt = DateTime.UtcNow
        };
        var result = await userMgr.CreateAsync(admin, "Admin@123"); // sicheres Passwort wählen!
        if (result.Succeeded)
            await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
    }
}



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Nach var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    await SeedRolesAndAdminAsync(scope.ServiceProvider);
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();    // wendet alle ausstehenden Migrationen an
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
