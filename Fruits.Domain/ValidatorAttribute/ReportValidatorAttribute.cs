using System.ComponentModel.DataAnnotations;
using Fruits.Domain.DB;
using Fruits.Domain.Report;

namespace Fruits.Domain.ValidatorAttribute;

/// <summary>
/// Валидатор Диапазона дат для выгрузки отчета
/// </summary>
public class ReportValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is ModelReport report)
        {
	        var dataMin = new DateTime(2000, 1, 1);
	        var dataMax = new DateTime(2099, 12, 31);

	        if (validationContext.MemberName == nameof(ModelReport.StartDate))
	        {
		        if (report.StartDate > report.EndDate)
		        {
			        return new ValidationResult("Дата 'Период С' должна быть меньше даты 'Период ПО'",
				        new List<string> { nameof(ModelReport.StartDate) });
		        }
		        if (report.StartDate < dataMin || report.StartDate > dataMax)
		        {
			        return new ValidationResult($"Дата 'Период С' должна быть в диапазоне! [{dataMin.ToString("d")}-{dataMax.ToString("d")}]",
				        new List<string> { nameof(PriceCatalog.StartDate) });
		        }
	        }

	        if (validationContext.MemberName == nameof(ModelReport.EndDate))
	        {
		        if (report.EndDate < report.StartDate)
		        {
			        return new ValidationResult("Дата 'Период ПО' должна быть больше даты 'Период С'",
				        new List<string> { nameof(ModelReport.EndDate) });
		        }
		        if (report.EndDate < dataMin || report.EndDate > dataMax)
		        {
			        return new ValidationResult($"Дата 'Период ПО' должна быть в диапазоне! [{dataMin.ToString("d")}-{dataMax.ToString("d")}]",
				        new List<string> { nameof(ModelReport.EndDate) });
		        }
	        }
        }


		return ValidationResult.Success;
    }
}