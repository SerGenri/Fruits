using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Fruits.Wpf.Core;

public static class Program
{
	[STAThread]
	public static void Main()
	{
		var app = new App();
		app.InitializeComponent();
		app.Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args)
	{
		var hostBuilder = Host.CreateDefaultBuilder(args);
		hostBuilder.UseContentRoot(App.CurrentDirectory);
		hostBuilder.ConfigureServices(App.ConfigureServices);
		return hostBuilder;
	}
	
}