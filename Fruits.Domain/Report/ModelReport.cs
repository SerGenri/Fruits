using Fruits.Domain.ValidatorAttribute;
using System.ComponentModel.DataAnnotations;

namespace Fruits.Domain.Report;

/// <summary>
/// Модель для формирования отчета
/// </summary>
public class ModelReport
{
	/// <summary>
	/// Начальная дата периода 
	/// </summary>
	[Required, ReportValidator]
	public DateTime StartDate { get; set; }

	/// <summary>
	/// Конечная дата периода
	/// </summary>
	[Required, ReportValidator]
	public DateTime EndDate { get; set; }
}