using BlazorPro.BlazorSize;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Fruits.DAL.Context;
using Fruits.Interfaces;
using Fruits.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

#region Выбор строки подключения
// Читаем строку подключения
string baseDirectory = AppContext.BaseDirectory;
string connectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=" + baseDirectory + "Data\\DatabaseFruits.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True";
#endregion

services.AddDbContext<FruitDbContext>(options =>
{
	options.UseSqlServer(connectionString);
	options.EnableSensitiveDataLogging();
}, ServiceLifetime.Transient);
services.AddDatabaseDeveloperPageExceptionFilter();
services.AddTransient<IDbInitializer, DbInitializer>();

// Add services to the container.
services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

// Forwarded Headers: 
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

services.AddRazorPages();
services.AddServerSideBlazor();

services.AddHotKeys2();
services.AddResizeListener(options =>
{
    options.ReportRate = 300;
    options.EnableLogging = false;
    options.SuppressInitEvent = true;
});
services.AddMediaQueryService();
services.AddHttpContextAccessor();

services.AddScoped<IMyIpService, MyIpService>();
services.AddScoped<IFriutServices, FriutServices>();
services.AddScoped<IReportServices, ReportServices>();

var app = builder.Build();

await using (var scop = app.Services.CreateAsyncScope())
{
	// Читаем параметр из файла конфигурации
	// Генерить тестовые данные
	var initDb = true;
	var generateData = builder.Configuration.GetSection("DataBase").GetSection("GenerateData").Value;

	bool.TryParse(generateData, out initDb);
	// Читаем параметр из файла конфигурации
	// Удалить все данные в базе
	var removeBefore = false;
	var removeData = builder.Configuration.GetSection("DataBase").GetSection("RemoveData").Value;
	bool.TryParse(removeData, out removeBefore);

	var dbInitializer = scop.ServiceProvider.GetRequiredService<IDbInitializer>();
	await dbInitializer.InitializeAsync(removeBefore, initDb);
}
  
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ContextMiddleware>();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
