using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Fruits.Interfaces;
using Fruits.Wpf.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fruits.Wpf.Core
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public static bool IsDesignMode { get; private set; } = true;

		private static IHost? _host;
		public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

		protected override async void OnStartup(StartupEventArgs e)
		{
			IsDesignMode = false;
		  
			var host = Host;

			base.OnStartup(e);

			await host.StartAsync().ConfigureAwait(false);
		}

		protected override async void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			var host = Host;

			await host.StartAsync().ConfigureAwait(false);

			host.Dispose();
			_host = null;
		}

		public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
		{
			services
				.RegisterServices()
				.RegisterViewModel();
		}

		public static string CurrentDirectory => IsDesignMode 
			? Path.GetDirectoryName(GetSourceCodePath()) 
			: Environment.CurrentDirectory;

		private static string GetSourceCodePath([CallerFilePath] string path = null) => path;
	}
}
