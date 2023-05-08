using Fruits.DAL.Context;
using Fruits.Domain.DB;
using Fruits.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fruits.Services;

/// <summary>
/// Создание БД
/// </summary>
public class DbInitializer : IDbInitializer
{
	private readonly FruitDbContext _db;

	public DbInitializer(FruitDbContext db)
	{
		_db = db;
	}

	/// <summary>
	/// Инициализация БД
	/// </summary>
	/// <param name="removeBefore">Удалить содержимое базы</param>
	/// <param name="initDb">Заполнять тестовыми данными</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task InitializeAsync(bool removeBefore, bool initDb, CancellationToken cancel = default)
	{
		if ((await _db.Database.GetPendingMigrationsAsync(cancel)).Any())
		{
			await _db.Database.MigrateAsync(cancel);
		}

		if (removeBefore)
		{
			await RemoveAsync(cancel);
		}

		if (initDb)
		{
			await InitDbAsync(cancel);
		}
	}

	/// <summary>
	/// Заполняем базу
	/// </summary>
	private async Task InitDbAsync(CancellationToken cancel = default)
	{
		if (!await _db.ProvidersCatalog.AnyAsync(cancel) 
		    && !await _db.FruitsCatalog.AnyAsync(cancel)
		    && !await _db.PriceCatalog.AnyAsync(cancel)
		    && !await _db.Stock.AnyAsync(cancel))
		{
			// Заполняем поставщиков
			List<ProvidersCatalog> providers = new List<ProvidersCatalog>
			{
				new() { NameProvider = "ИП Иванов" },
				new() { NameProvider = "ИП Петров" },
				new() { NameProvider = "ИП Сидоров" }
			};

			// Заполняем Товары
			List<FruitsCatalog> fruits = new List<FruitsCatalog>
			{
				new() { Class = "Груши", Sort = "Чижовская" },
				new() { Class = "Груши", Sort = "Джулия" },
				new() { Class = "Груши", Sort = "Красуля" },
				new() { Class = "Груши", Sort = "Кафедральная" },
				new() { Class = "Груши", Sort = "Северянка" },
				new() { Class = "Груши", Sort = "Виктория" },
				new() { Class = "Яблоки", Sort = "Антоновка" },
				new() { Class = "Яблоки", Sort = "Бреберн" },
				new() { Class = "Яблоки", Sort = "Фуджи" },
				new() { Class = "Яблоки", Sort = "Гала" },
				new() { Class = "Яблоки", Sort = "Голден" },
				new() { Class = "Яблоки", Sort = "Грэнни Смит" }
			};

			// Заполняем План поставки
			List<PriceCatalog> prices = new List<PriceCatalog>();
			foreach (var provider in providers)
			{
				Random rndPrice = new Random();
				Random rndFruit = new Random();
				Random rndData = new Random();
							    
				for (int i = 0; i < 5; i++)
				{
					var pc = new PriceCatalog
					{
						StartDate = DateTime.Now.Date.AddMonths(rndData.Next(-10, -1)),
						EndDate = DateTime.Now.Date.AddMonths(rndData.Next(1, 10)),
						Price = rndPrice.Next(10, 1000),
						Provider = provider,
						Fruit = fruits[rndFruit.Next(0, fruits.Count - 1)]
					};

					prices.Add(pc);
				}
			}

			// Поставки
			List<Stock> ctocks = new List<Stock>();
			foreach (var provider in providers)
			{
				Random rndData = new Random();
				Random rndMass = new Random();
				Random rndPrice = new Random();
				Random rndSf = new Random();

				for (int i = 0; i < 2; i++)
				{
					 var stockObj = new Stock();
					 stockObj.DeliveryDate = DateTime.Now.Date.AddMonths(rndData.Next(-5, 5));
					 stockObj.Provider = provider;

					// Заполняем Состав поставки
					List<StockFruits> sfList = new List<StockFruits>();
					foreach (var fr in fruits.Take(rndSf.Next(1, fruits.Count - 1)))
					{
						var sfObj = new StockFruits();
						sfObj.Fruit = fr;
						sfObj.Stock = stockObj;
						sfObj.Mass = rndMass.Next(1, 1000);
						sfObj.Price = rndPrice.Next(50, 1000);

						sfList.Add(sfObj);
					}

					stockObj.ListStockFruits = new List<StockFruits>(sfList);
					ctocks.Add(stockObj);
				}
			}

			await _db.AddRangeAsync(providers, cancel);
			await _db.AddRangeAsync(fruits, cancel);
			await _db.AddRangeAsync(prices, cancel);
			await _db.AddRangeAsync(ctocks, cancel);
			await _db.SaveChangesAsync(cancel);

			// Обновляем цены из графика поставок
			foreach (var stockFruit in _db.StockFruits)
			{
				stockFruit.GetPriceFromCatDb();
			}
			await _db.SaveChangesAsync(cancel);
		}
	}

	/// <summary>
	/// Удаляем содержимое
	/// </summary>
	private async Task RemoveAsync(CancellationToken cancel = default)
	{
		if (_db.Stock.Any()) _db.Stock.RemoveRange(_db.Stock);
		if (_db.FruitsCatalog.Any()) _db.FruitsCatalog.RemoveRange(_db.FruitsCatalog);
		if (_db.PriceCatalog.Any()) _db.PriceCatalog.RemoveRange(_db.PriceCatalog);
		if (_db.ProvidersCatalog.Any()) _db.ProvidersCatalog.RemoveRange(_db.ProvidersCatalog);
		if (_db.StockFruits.Any()) _db.StockFruits.RemoveRange(_db.StockFruits);

		await _db.SaveChangesAsync(cancel);
	}
}