using System.ComponentModel.DataAnnotations;
using Fruits.Domain.DB;

namespace Fruits.Domain.ValidatorAttribute;

/// <summary>
/// Валидатор Диапазона поставки
/// </summary>
public class PriceCatalogValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is PriceCatalog priceCatalog)
        {
            var dataMin = new DateTime(2000, 1, 1);
            var dataMax = new DateTime(2099, 12, 31);

            if (validationContext.MemberName == nameof(PriceCatalog.StartDate))
            {
                if (priceCatalog.StartDate > priceCatalog.EndDate)
                {
	                return new ValidationResult("Дата 'Период С' должна быть меньше даты 'Период ПО'",
		                new List<string> { nameof(PriceCatalog.StartDate) });
                }
				if (priceCatalog.StartDate < dataMin || priceCatalog.StartDate > dataMax)
                {
	                return new ValidationResult($"Дата 'Период С' должна быть в диапазоне! [{dataMin.ToString("d")}-{dataMax.ToString("d")}]",
		                new List<string> { nameof(PriceCatalog.StartDate) });
                }
			}

			if (validationContext.MemberName == nameof(PriceCatalog.EndDate))
            {
                if (priceCatalog.EndDate < priceCatalog.StartDate)
                {
	                return new ValidationResult("Дата 'Период ПО' должна быть больше даты 'Период С'",
		                new List<string> { nameof(PriceCatalog.EndDate) });
                }
				if (priceCatalog.EndDate < dataMin || priceCatalog.EndDate > dataMax)
                {
	                return new ValidationResult($"Дата 'Период ПО' должна быть в диапазоне! [{dataMin.ToString("d")}-{dataMax.ToString("d")}]",
		                new List<string> { nameof(PriceCatalog.EndDate) });
                }
			}
        }

		return ValidationResult.Success;
    }
}