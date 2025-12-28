using QL_DienTuGiaDung.BLL;
using QL_DienTuGiaDung.DAL;
using QL_DienTuGiaDung.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;                
    options.Cookie.IsEssential = true;          
});

builder.Services.AddAuthentication("VElectricCookie")
    .AddCookie("VElectricCookie", options =>
    {
        options.LoginPath = "/Account/Index";
        options.AccessDeniedPath = "/Account/Denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;

        options.Cookie.Name = "VElectricAuth";
        options.Cookie.HttpOnly = true;

        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/Admin"))
                context.Response.Redirect("/Admin/Login");
            else
                context.Response.Redirect("/Account/Index");

            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AccountBLL>();
builder.Services.AddScoped<ProductBLL>();
builder.Services.AddScoped<PaymentMethodBLL>();
builder.Services.AddScoped<OrderBLL>();
builder.Services.AddScoped<DeliveryAddressBLL>();
builder.Services.AddScoped<ThuongHieuBLL>();
builder.Services.AddScoped<QuocGiaBLL>();
builder.Services.AddScoped<ThongKeBLL>();

builder.Services.AddScoped<AccountDAL>();
builder.Services.AddScoped<ProductDAL>();
builder.Services.AddScoped<PaymentMethodDAL>();
builder.Services.AddScoped<OrderDAL>();
builder.Services.AddScoped<DeliveryAddressDAL>();
builder.Services.AddScoped<ThuongHieuDAL>();
builder.Services.AddScoped<QuocGiaDAL>();
builder.Services.AddScoped<ThongKeDAL>();

builder.Services.AddScoped<DatabaseHelper>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
