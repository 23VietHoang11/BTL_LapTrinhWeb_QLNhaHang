using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<QLNhaHangContext>(options =>
    options.UseSqlServer(connectionString));
var qlnhConnectionString = builder.Configuration.GetConnectionString("QLNhaHangConnection") ?? throw new InvalidOperationException("Connection string 'QLNhaHangConnection' not found.");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<QLNhaHangContext>();
builder.Services.AddControllersWithViews();

// Thêm dịch vụ Xác thực
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "H2AK_RestaurantAuth"; // Tên cookie
        options.LoginPath = "/Account/Login";     // Đường dẫn đến trang đăng nhập
        options.LogoutPath = "/Account/Logout";    // Đường dẫn đến trang đăng xuất
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang "Cấm truy cập"
        options.ExpireTimeSpan = TimeSpan.FromDays(1); // Thời gian cookie hết hạn
    });

// Thêm dịch vụ HTTP Context Accessor (để đọc Session/Cookie)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(); // Thêm Session (nếu bạn cần dùng)

//Upload file
builder.Services.AddTransient<IBufferedFileUploadService, BufferedFileUploadLocalService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// === KÍCH HOẠT XÁC THỰC ===
app.UseSession(); // Kích hoạt Session
app.UseAuthentication(); // BẮT BUỘC: Kích hoạt xác thực (đọc cookie)
app.UseAuthorization();  // BẮT BUỘC: Kích hoạt phân quyền


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
