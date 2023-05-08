using Fruits.Domain;
using Fruits.Domain.DB;
using Microsoft.EntityFrameworkCore;

namespace Fruits.Interfaces;

/// <summary>
/// Сервис для работы с базой Фрукты
/// </summary>
public interface IFriutServices
{
	#region Get
	/// <summary>
	/// Выгрузка списка поставок
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<Stock[]> GetAllStock(bool lazyLoad = false, CancellationToken cancel = default);
	/// <summary>
	/// Выгрузка списка поставщиков
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<ProvidersCatalog[]> GetAllProvidersCatalog(bool lazyLoad = false, CancellationToken cancel = default);
	/// <summary>
	/// Выгрузка списка фруктов
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<FruitsCatalog[]> GetAllFruitsCatalog(bool lazyLoad = false, CancellationToken cancel = default);
	/// <summary>
	/// Выгрузка списка Графика поставок
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<PriceCatalog[]> GetAllPriceCatalog(bool lazyLoad = false, CancellationToken cancel = default);
	/// <summary>
	/// Выгрузка списка Комплекта поставок
	/// </summary>
	/// <param name="lazyLoad">Загрузка без отмены изменений</param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<StockFruits[]> GetAllStockFruits(bool lazyLoad = false, CancellationToken cancel = default);
	#endregion

	#region Remove
	/// <summary>
	/// Удалить Строку из комплекта поставки
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<bool> RemoveStockFruitsAsync(StockFruits item, CancellationToken cancel = default);
	/// <summary>
	/// Удалить Строку из поставки
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<bool> RemoveStockAsync(Stock item, CancellationToken cancel = default);
	/// <summary>
	/// Удалить Строку из списка поставщиков
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<RemoveResult> RemoveProvidersCatalogAsync(ProvidersCatalog item, CancellationToken cancel = default);
	/// <summary>
	/// Удалить Строку из Графика поставок
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<bool> RemovePriceCatalogAsync(PriceCatalog item, CancellationToken cancel = default);
	/// <summary>
	/// Удалить Строку из спика фруктов
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<RemoveResult> RemoveFruitsCatalogAsync(FruitsCatalog item, CancellationToken cancel = default);
	#endregion

	#region Add
	/// <summary>
	/// Добавить строку в Комплект поставки
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task AddStockFruitsAsync(StockFruits item, CancellationToken cancel = default);
	/// <summary>
	/// Добавить Поставку
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task AddStockAsync(Stock item, CancellationToken cancel = default);
	/// <summary>
	/// Добавить строку в список поставщиков
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task AddProvidersCatalogAsync(ProvidersCatalog item, CancellationToken cancel = default);
	/// <summary>
	/// Добавить строку в График поставок
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task AddPriceCatalogAsync(PriceCatalog item, CancellationToken cancel = default);
	/// <summary>
	/// Добавить строку в список фруктов
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task AddFruitsCatalogAsync(FruitsCatalog item, CancellationToken cancel = default);
	#endregion

	#region Update
	/// <summary>
	/// Сохранить изменения в поставке 
	/// </summary>
	/// <param name="item"></param>
	public void UpdateStock(Stock item);
	/// <summary>
	/// Сохранить изменения в Комплекте поставки 
	/// </summary>
	/// <param name="item"></param>
	public void UpdateStockFruits(StockFruits item);
	#endregion

	#region UndoChanges
	/// <summary>
	/// Откатить все изменения
	/// </summary>
	public void UndoAllChanges();

	/// <summary>
	/// Отмена изменений конкретной сущности
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cancel"></param>
	public Task UndoChangesItemAsync<T>(T? item, CancellationToken cancel = default);

	/// <summary>
	/// Отмена изменений конкретной таблицы
	/// </summary>
	/// <param name="listDbSet"></param>
	/// <param name="cancel"></param>
	public Task UndoChangesTableAsync<T>(DbSet<T>? listDbSet, CancellationToken cancel = default) where T : class;
	#endregion

	/// <summary>
	/// Записать изменения в БД
	/// </summary>
	/// <param name="cancel"></param>
	/// <returns></returns>
	public Task<bool> SaveChangesAsync(CancellationToken cancel = default);
}