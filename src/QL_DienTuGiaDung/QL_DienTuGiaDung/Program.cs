using QL_DienTuGiaDung.Helpers;
using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.BLL;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;                
    options.Cookie.IsEssential = true;          
});

builder.Services
    .AddAuthentication("VElectricCookie")
    .AddCookie("VElectricCookie", options =>
    {
        options.LoginPath = "/TaiKhoan/Index";
        options.AccessDeniedPath = "/SanPham/Index";

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToAccessDenied = context =>
            {
                context.Response.Redirect("/SanPham/Index");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DatabaseHelper>();

builder.Services.AddScoped<LoaiSanPhamDAL>();
builder.Services.AddScoped<SanPhamDAL>();
builder.Services.AddScoped<TaiKhoanDAL>();
builder.Services.AddScoped<DiaChiDAL>();
builder.Services.AddScoped<AnhDAL>();
builder.Services.AddScoped<DanhGiaDAL>();
builder.Services.AddScoped<DonHangDAL>();
builder.Services.AddScoped<ThongKeDAL>();
builder.Services.AddScoped<QuocGiaDAL>();
builder.Services.AddScoped<ThuongHieuDAL>();

builder.Services.AddScoped<LoaiSanPhamBLL>();
builder.Services.AddScoped<SanPhamBLL>();
builder.Services.AddScoped<TaiKhoanBLL>();
builder.Services.AddScoped<DiaChiBLL>();
builder.Services.AddScoped<AnhBLL>();
builder.Services.AddScoped<DanhGiaBLL>();
builder.Services.AddScoped<DonHangBLL>();
builder.Services.AddScoped<ThongKeBLL>();
builder.Services.AddScoped<QuocGiaBLL>();
builder.Services.AddScoped<ThuongHieuBLL>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();          
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SanPham}/{action=Index}/{id?}");

app.Run();
