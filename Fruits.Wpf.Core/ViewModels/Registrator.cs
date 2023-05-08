using Fruits.DAL.Context;
using Fruits.Interfaces;
using Fruits.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fruits.Wpf.Core.ViewModels;

internal static class Registrator
{
	public static IServiceCollection RegisterViewModel(this IServiceCollection services)
	{
		services.AddTransient<ViewModelMain>();
		services.AddTransient<ViewModelFruitsCatalog>();
		services.AddTransient<ViewModelPriceCatalog>();
		services.AddTransient<ViewModelProvidersCatalog>();
		services.AddTransient<ViewModelReport>();

		return services;
	}

	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		// Читаем строку подключения
		string baseDirectory = App.CurrentDirectory;
		string connectionString;
		if (App.IsDesignMode)
		{
			connectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=" + baseDirectory + "\\DatabaseFruits.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True";
		}
		else
		{
			connectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=" + baseDirectory + "\\Data\\DatabaseFruits.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True";
		}

		services.AddDbContext<FruitDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
			options.EnableSensitiveDataLogging();
		});

		services.AddScoped<IDbInitializer, DbInitializer>();
		services.AddScoped<IFriutServices, FriutServices>();
		services.AddScoped<IReportServices, ReportServices>();

		return services;
	}
}