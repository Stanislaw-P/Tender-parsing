using Serilog;
using Tender_parsing.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("MarketMosregApi", client =>
{
    client.BaseAddress = new Uri("https://api.market.mosreg.ru");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("MarketMosregWeb", client =>
{
    client.BaseAddress = new Uri("https://market.mosreg.ru");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<IMarketMosregApiClient, MarketMosregApiClient>();
builder.Services.AddScoped<ITenderService, TenderService>();
builder.Services.AddScoped<ITenderHtmlParser, TenderHtmlParser>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
