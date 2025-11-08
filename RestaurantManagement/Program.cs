using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entities;


var builder = WebApplication.CreateBuilder(args);

// ===== KẾT NỐI DATABASE =====
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<QLNhaHangContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ===== CẤU HÌNH IDENTITY =====
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<QLNhaHangContext>();

// ===== DỊCH VỤ MVC / API =====
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ===== CẤU HÌNH COOKIE AUTH =====
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "H2AK_RestaurantAuth";
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

// ===== SESSION =====
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

// ===== UPLOAD FILE SERVICE =====
builder.Services.AddTransient<IBufferedFileUploadService, BufferedFileUploadLocalService>();

var app = builder.Build();

// ===== PIPELINE =====
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ===== MAP ROUTES =====
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


// ⚠️ Rất quan trọng: map các API controller
app.MapControllers();

app.MapRazorPages();

app.Run();
