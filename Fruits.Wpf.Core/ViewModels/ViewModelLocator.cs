using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fruits.Wpf.Core.ViewModels;

public class ViewModelLocator
{
	private IServiceProvider Services => App.Host.Services;

	public ViewModelMain ViewModelMain => Services.GetRequiredService<ViewModelMain>();
	public ViewModelFruitsCatalog ViewModelFruitsCatalog => Services.GetRequiredService<ViewModelFruitsCatalog>();
	public ViewModelPriceCatalog ViewModelPriceCatalog => Services.GetRequiredService<ViewModelPriceCatalog>();
	public ViewModelProvidersCatalog ViewModelProvidersCatalog => Services.GetRequiredService<ViewModelProvidersCatalog>();
	public ViewModelReport ViewModelReport => Services.GetRequiredService<ViewModelReport>();
	
}