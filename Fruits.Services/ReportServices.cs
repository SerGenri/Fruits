using Fruits.Domain.DB;
using Fruits.Interfaces;
using Fruits.Domain.Report;

namespace Fruits.Services;

/// <summary>
/// Сервис для формирования отчетов
/// </summary>
public class ReportServices : IReportServices
{
	private readonly IFriutServices _db;

	public ReportServices(IFriutServices db)
	{
		_db = db;
	}

	/// <summary>
	/// Выгрузка отчета по фруктам
	/// </summary>
	/// <param name="startDate"></param>
	/// <param name="endDate"></param>
	/// <returns></returns>
	public async Task<List<Report>> GetReportAsync(DateTime startDate, DateTime endDate)
	{
		var listReport = new List<Report>();

		var providers = await _db.GetAllProvidersCatalog(true);
		await _db.GetAllStockFruits(true);

		//Проходим по таблицам от поставщика до его заказов
		List<ProvidersCatalog> objProvidersCatalog = new List<ProvidersCatalog>(providers);
		foreach (ProvidersCatalog provider in objProvidersCatalog)
		{
			ICollection<Stock> listStock = provider.ListStock;
			foreach (Stock stock in listStock)
			{
				//проверяем диапазон дат
				if (stock.DeliveryDate.Date >= startDate.Date && stock.DeliveryDate.Date <= endDate.Date)
				{
					ICollection<StockFruits> listStockFruits = stock.ListStockFruits;
					foreach (StockFruits itemStockFruit in listStockFruits)
					{
						//Проверяем наличие записи в листе, если нет добавляем, если есть плюсуем к существующей
						if (listReport.Any(x => x.NameProvider == provider.NameProvider && x.NameFruit == itemStockFruit.FullName))
						{
							Report objReport = listReport.First(x => x.NameProvider == provider.NameProvider && x.NameFruit == itemStockFruit.FullName);
							objReport.MassSumm += itemStockFruit.Mass;
							objReport.PriceSumm += itemStockFruit.Mass * itemStockFruit.Price;
						}
						else
						{
							listReport.Add(new Report
							{
								NameProvider = provider.NameProvider,
								NameFruit = itemStockFruit.FullName,
								MassSumm = itemStockFruit.Mass,
								PriceSumm = itemStockFruit.Mass * itemStockFruit.Price
							});
						}
					}
				}
			}
		}

		return listReport;
	}

}