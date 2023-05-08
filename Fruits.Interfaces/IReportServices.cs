using Fruits.Domain.Report;

namespace Fruits.Interfaces;

/// <summary>
/// Сервис для формирования отчетов
/// </summary>
public interface IReportServices
{
	/// <summary>
	/// Выгрузка отчета по фруктам
	/// </summary>
	/// <param name="startDate"></param>
	/// <param name="endDate"></param>
	/// <returns></returns>
	public Task<List<Report>> GetReportAsync(DateTime startDate, DateTime endDate);
}