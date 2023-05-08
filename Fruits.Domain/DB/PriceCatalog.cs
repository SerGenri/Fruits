using Fruits.Domain.ValidatorAttribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// График поставок
	/// </summary>
    public class PriceCatalog : Base.Base, IDataErrorInfo
	{
	    private DateTime _startDate;
	    private DateTime _endDate;
	    private double _price;
	    private FruitsCatalog _fruit;
	    private ProvidersCatalog _provider;
	    private int _idFruitsCatalog;
	    private int _idProviderCatalog;

	    public PriceCatalog()
	    {
		    _fruit = new FruitsCatalog();
		    _provider = new ProvidersCatalog();
	    }

		public PriceCatalog(FruitsCatalog fruit, ProvidersCatalog provider)
		{
			StartDate = DateTime.Now.Date;
			EndDate = DateTime.Now.Date;

		    _fruit = fruit;
		    _provider = provider;
	    }

	    [Key]
        public int IdPriceCatalog { get; set; }
			
		/// <summary>
		/// Начальная дата периода 
		/// </summary>
		[Required, PriceCatalogValidator]
		public DateTime StartDate
        {
	        get => _startDate;
	        set => Set(ref _startDate, value);
        }

		/// <summary>
		/// Конечная дата периода
		/// </summary>
		[Required, PriceCatalogValidator]
		public DateTime EndDate
        {
	        get => _endDate;
	        set => Set(ref _endDate, value);
        }

		/// <summary>
		/// Цена за фрукт в периоде поставки
		/// </summary>
        [Required, Column(TypeName = "decimal(18,2)"), Range(1, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
		public double Price
        {
	        get => _price;
	        set => Set(ref _price, value);
        }

		public int IdFruitsCatalog
		{
			get => _idFruitsCatalog;
			set => Set(ref _idFruitsCatalog, value);
		}

		/// <summary>
		/// Фрукт
		/// </summary>
		[ForeignKey(nameof(IdFruitsCatalog))]
		public virtual FruitsCatalog Fruit
        {
	        get => _fruit;
	        set => Set(ref _fruit, value);
        }

		public int IdProviderCatalog
		{
			get => _idProviderCatalog;
			set => Set(ref _idProviderCatalog, value);
		}

		/// <summary>
		/// Поставщик
		/// </summary>
		[ForeignKey(nameof(IdProviderCatalog))]
		public virtual ProvidersCatalog Provider
        {
	        get => _provider;
	        set => Set(ref _provider, value);
        }

		#region Validation WPF
		[NotMapped]
		public List<string> ListHasErrorProperty { get; set; } = new();

		[NotMapped]
		public string Error { get; } = string.Empty;

		[NotMapped]
		public string this[string columnName]
		{
			get
			{
				var dataMin = new DateTime(2000, 1, 1);
				var dataMax = new DateTime(2099, 12, 31);

				string error = string.Empty;
				switch (columnName)
				{
					case nameof(Price):
						//Обработка ошибок для свойства Price
						if (Price == 0 || Price < 0)
						{
							error = "Сумма должна быть больше 0!";
						}
						break;
					case nameof(StartDate):
						//Обработка ошибок для свойства StartDate
						if (StartDate > EndDate || StartDate < dataMin || StartDate > dataMax)
						{
							error = $"Дата должна быть в диапазоне! [{dataMin.ToString("d")}-{dataMax.ToString("d")}] и меньше даты ПО";
						}
						break;
					case nameof(EndDate):
						//Обработка ошибок для свойства EndDate
						if (EndDate < StartDate || EndDate < dataMin || EndDate > dataMax) 
						{
							error = $"Дата должна быть в диапазоне! [{dataMin.ToString("d")}-{dataMax.ToString("d")}] и больше даты С";
						}
						break;
				}

				if (!string.IsNullOrEmpty(error))
				{
					if (!ListHasErrorProperty.Contains(columnName))
					{
						ListHasErrorProperty.Add(columnName);
					}
				}
				else
				{
					if (ListHasErrorProperty.Contains(columnName))
					{
						ListHasErrorProperty.Remove(columnName);
					}
				}
				OnPropertyChanged(nameof(ListHasErrorProperty));

				return error;
			}
		}
		#endregion
	}
}
