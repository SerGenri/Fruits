using Fruits.Interfaces;
using Fruits.Domain.DB;
using Fruits.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Fruits.Domain;

namespace Fruits.Services;

/// <summary>
/// Сервис для работы с базой Фрукты
/// </summary>
public class FriutServices : IFriutServices
{
	private readonly FruitDbContext _db;

	public FriutServices(FruitDbContext db)
	{
		_db = db;
	}

	#region Get
	/// <summary>
	/// Выгрузка списка поставок
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<Stock[]> GetAllStock(bool lazyLoad = false, CancellationToken cancel = default)
	{
		if (!lazyLoad)
		{
			await UndoChangesTableAsync(_db.Stock, cancel);
		}

		return await _db.Stock
			.Include(x=> x.Provider)
			.Include(x => x.ListStockFruits)
			.OrderBy(x=>x.DeliveryDate)
			.ThenBy(x=>x.Provider.NameProvider)
			.ToArrayAsync(cancel);
	}
	/// <summary>
	/// Выгрузка списка поставщиков
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<ProvidersCatalog[]> GetAllProvidersCatalog(bool lazyLoad = false, CancellationToken cancel = default)
	{
		if (!lazyLoad)
		{
			await UndoChangesTableAsync(_db.ProvidersCatalog, cancel);
		}

		return await _db.ProvidersCatalog
			.Include(x => x.ListPriceCatalog)
			.Include(x => x.ListStock)
			.OrderBy(x => x.NameProvider)
			.ToArrayAsync(cancel);
	}
	/// <summary>
	/// Выгрузка списка фруктов
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<FruitsCatalog[]> GetAllFruitsCatalog(bool lazyLoad = false, CancellationToken cancel = default)
	{
		if (!lazyLoad)
		{
			await UndoChangesTableAsync(_db.FruitsCatalog, cancel);
		}

		return await _db.FruitsCatalog
			.Include(x => x.ListPriceCatalog)
			.Include(x => x.ListStockFruits)
			.OrderBy(x => x.Class)
			.ThenBy(x => x.Sort)
			.ToArrayAsync(cancel);
	}
	/// <summary>
	/// Выгрузка списка Графика поставок
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<PriceCatalog[]> GetAllPriceCatalog(bool lazyLoad = false, CancellationToken cancel = default)
	{
		if (!lazyLoad)
		{
			await UndoChangesTableAsync(_db.PriceCatalog, cancel);
		}

		return await _db.PriceCatalog
			.Include(x => x.Fruit)
			.Include(x => x.Provider)
			.OrderBy(x => x.StartDate)
			.ThenBy(x => x.EndDate)
			.ThenBy(x => x.Provider.NameProvider)
			.ThenBy(x => x.Fruit.Class)
			.ThenBy(x => x.Fruit.Sort)
			.ToArrayAsync(cancel);
	}
	/// <summary>
	/// Выгрузка списка Комплекта поставок
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<StockFruits[]> GetAllStockFruits(bool lazyLoad = false, CancellationToken cancel = default)
	{
		if (!lazyLoad)
		{
			await UndoChangesTableAsync(_db.StockFruits, cancel);
		}

		return await _db.StockFruits
			.Include(x => x.Fruit)
			.Include(x => x.Stock)
			.OrderBy(x => x.Fruit!.Class)
			.ThenBy(x => x.Fruit!.Sort)
			.ThenBy(x => x.Mass)
			.ThenBy(x => x.Price)
			.ToArrayAsync(cancel);
	}
	#endregion

	#region Remove
	/// <summary>
	/// Удалить Строку из комплекта поставки
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<bool> RemoveStockFruitsAsync(StockFruits item, CancellationToken cancel = default)
	{
		if (await _db.StockFruits.ContainsAsync(item, cancel).ConfigureAwait(false) || item.IdStockFruits == 0)
		{
			//_db.Entry(item).State = EntityState.Deleted;
			_db.Remove(item);
			return true;
		}

		return false;
	}
	/// <summary>
	/// Удалить Строку из поставки
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<bool> RemoveStockAsync(Stock item, CancellationToken cancel = default)
	{
		if (await _db.Stock.ContainsAsync(item, cancel).ConfigureAwait(false) || item.IdStock == 0)
		{
			_db.RemoveRange(item.ListStockFruits);

			//_db.Entry(item).State = EntityState.Deleted;
			_db.Remove(item);
			return true;
		}

		return false;
	}
	/// <summary>
	/// Удалить Строку из списка поставщиков
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<RemoveResult> RemoveProvidersCatalogAsync(ProvidersCatalog item, CancellationToken cancel = default)
	{
		try
		{
			if (await _db.ProvidersCatalog.ContainsAsync(item, cancel).ConfigureAwait(false) || item.IdProviderCatalog == 0)
			{
				//_db.Entry(item).State = EntityState.Deleted;
				_db.Remove(item);
				return new RemoveResult(true);
			}

			return new RemoveResult(false);
		}
		catch (Exception e)
		{
			UndoAllChanges();

			string error;

			if (e.Message.Contains("foreign key") && e.Message.Contains("cascade deletes"))
			{
				error = $"Нельзя удалить используемый элемент!\r\n{item.NameProvider}";
			}
			else
			{
				error = e.Message;
			}

			return new RemoveResult(false, error);
		}
	}
	/// <summary>
	/// Удалить Строку из Графика поставок
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<bool> RemovePriceCatalogAsync(PriceCatalog item, CancellationToken cancel = default)
	{
		if (await _db.PriceCatalog.ContainsAsync(item, cancel).ConfigureAwait(false) || item.IdPriceCatalog == 0)
		{
			//_db.Entry(item).State = EntityState.Deleted;
			_db.Remove(item);
			return true;
		}

		return false;
	}
	/// <summary>
	/// Удалить Строку из спика фруктов
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<RemoveResult> RemoveFruitsCatalogAsync(FruitsCatalog item, CancellationToken cancel = default)
	{
		try
		{
			if (await _db.FruitsCatalog.ContainsAsync(item, cancel).ConfigureAwait(false) || item.IdFruitsCatalog == 0)
			{
				//_db.Entry(item).State = EntityState.Deleted;
				_db.Remove(item);
				return new RemoveResult(true);
			}

			return new RemoveResult(false);
		}
		catch (Exception e)
		{
			UndoAllChanges();

			string error;

			if (e.Message.Contains("foreign key") && e.Message.Contains("cascade deletes"))
			{
				error = $"Нельзя удалить используемый элемент!\r\n{item.FullName}";
			}
			else
			{
				error = e.Message;
			}

			return new RemoveResult(false, error);
		}
	}
	#endregion

	#region Add
	/// <summary>
	/// Добавить строку в Комплект поставки
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task AddStockFruitsAsync(StockFruits item, CancellationToken cancel = default)
	{
		await _db.AddAsync(item, cancel).ConfigureAwait(false);
	}
	/// <summary>
	/// Добавить Поставку
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task AddStockAsync(Stock item, CancellationToken cancel = default)
	{
		await _db.AddAsync(item, cancel).ConfigureAwait(false);
	}
	/// <summary>
	/// Добавить строку в список поставщиков
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task AddProvidersCatalogAsync(ProvidersCatalog item, CancellationToken cancel = default)
	{
		await _db.AddAsync(item, cancel).ConfigureAwait(false);
	}
	/// <summary>
	/// Добавить строку в График поставок
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task AddPriceCatalogAsync(PriceCatalog item, CancellationToken cancel = default)
	{
		await _db.AddAsync(item, cancel).ConfigureAwait(false);
	}
	/// <summary>
	/// Добавить строку в список фруктов
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task AddFruitsCatalogAsync(FruitsCatalog item, CancellationToken cancel = default)
	{
		await _db.AddAsync(item, cancel).ConfigureAwait(false);
	}
	#endregion

	#region Update
	/// <summary>
	/// Сохранить изменения в поставке 
	/// </summary>
	/// <param name="item"></param>
	public void UpdateStock(Stock item)
	{
		_db.Update(item);
	}
	/// <summary>
	/// Сохранить изменения в Комплекте поставки 
	/// </summary>
	/// <param name="item"></param>
	public void UpdateStockFruits(StockFruits item)
	{
		_db.Update(item);
	}
	#endregion

	#region UndoChanges
	/// <summary>
	/// Откатить все изменения
	/// </summary>
	public void UndoAllChanges()
	{
		//обнаружить все изменения (вероятно, не требуется, если для параметра AutoDetectChanges установлено значение true)
		_db.ChangeTracker.DetectChanges();

		//получить все записи, которые были изменены
		var entries = _db.ChangeTracker.Entries()
			.Where(e => e.State != EntityState.Unchanged).ToList();

		//попытаться отменить изменения в каждой записи
		foreach (var dbEntityEntry in entries)
		{
			if (dbEntityEntry.State == EntityState.Added)
			{
				//если объект находится в состоянии "Добавлено", удалить его
				_db.Remove(dbEntityEntry);
			}
			else if (dbEntityEntry.State == EntityState.Modified)
			{
				//сущность изменена ... вы можете установить для нее значение «Без изменений» или «Перезагрузить» из базы данных
				dbEntityEntry.Reload();
			}
			else if (dbEntityEntry.State == EntityState.Deleted)
				//сущность удалена... не знаю, что с ней делать... установить для нее значение Modified или Unchanged
				dbEntityEntry.State = EntityState.Modified;
		}
	}

	/// <summary>
	/// Отмена изменений конкретной сущности
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	public async Task UndoChangesItemAsync<T>(T? item, CancellationToken cancel = default)
	{
		if (item == null)
		{
			return;
		}

		await _db.Entry(item).ReloadAsync(cancel);
	}

	/// <summary>
	/// Отмена изменений конкретной таблицы
	/// </summary>
	/// <param name="listDbSet"></param>
	/// <param name="cancel"></param>
	public async Task UndoChangesTableAsync<T>(DbSet<T>? listDbSet, CancellationToken cancel = default) where T : class
	{
		if (listDbSet == null)
		{
			return;
		}

		foreach (var item in listDbSet)
		{
			await _db.Entry(item).ReloadAsync(cancel);
		}
	}
	#endregion

	/// <summary>
	/// Записать изменения в БД
	/// </summary>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public async Task<bool> SaveChangesAsync(CancellationToken cancel = default)
	{
		return await _db.SaveChangesAsync(cancel).ConfigureAwait(false) != 0;
	}
}