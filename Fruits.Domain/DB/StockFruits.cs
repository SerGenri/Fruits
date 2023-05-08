using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// Комплект поставки
	/// </summary>
	public class StockFruits : Base.Base, IDataErrorInfo
	{
	    private double _price;
	    private int _mass;
	    private Stock _stock;
	    private FruitsCatalog? _fruit;
	    private bool _priceLbl;
	    private bool _priceCatalogLbl;
	    private int _idFruitsCatalog;

		#pragma warning disable CS8618
		public StockFruits()
		#pragma warning restore CS8618
	    {
		    Stock = new Stock();
		}

		[Key]
        public int IdStockFruits { get; set; }

		/// <summary>
		/// Цена за килограмм
		/// </summary>
        [Required, Column(TypeName = "decimal(18,2)"), Range(1, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
        public double Price
		{
			get
			{
				GetPriceFromCatDb();

				return _price;
			}
			set => Set(ref _price, value);
		}

		/// <summary>
		/// Вес, кг
		/// </summary>
		[Required, Range(1, int.MaxValue, ErrorMessage = "Вес должен быть больше 0")]
		public int Mass
        {
	        get => _mass;
	        set => Set(ref _mass, value);
        }

		[Range(1, int.MaxValue, ErrorMessage = "Выберите фрукт!")]
		public int IdFruitsCatalog
		{
			get => _idFruitsCatalog;
			set => Set(ref _idFruitsCatalog, value);
		}

		/// <summary>
		/// Фрукт
		/// </summary>
		[ForeignKey(nameof(IdFruitsCatalog))]
		public virtual FruitsCatalog? Fruit
        {
	        get => _fruit;
	        set
	        {
		        if (Set(ref _fruit, value))
		        {
			        OnPropertyChanged(nameof(FullName));
				}

		        GetPriceFromCatDb();
			}
		}

        public int IdStock { get; set; }

		/// <summary>
		/// Поставка
		/// </summary>
        [ForeignKey(nameof(IdStock))]
		public virtual Stock Stock
        {
	        get => _stock;
	        set => Set(ref _stock, value);
        }

		#region NotMapped

		[NotMapped]
		public string? FullName => Fruit?.FullName;


		[NotMapped]
		public bool PriceCatalogLbl
		{
			get => _priceCatalogLbl;
			set => Set(ref _priceCatalogLbl, value);
		}

		[NotMapped]
		public bool PriceLbl
		{
			get => _priceLbl;
			set => Set(ref _priceLbl, value);
		}

		/// <summary>
		/// Достаем цену из графика поставки
		/// </summary>
		public void GetPriceFromCatDb()
		{
			PriceCatalog? objCatalog = null;

			if (Fruit != null)
			{
				objCatalog = _stock.Provider.ListPriceCatalog.FirstOrDefault(x =>  
					x.StartDate <= Stock.DeliveryDate 
					&& x.EndDate >= Stock.DeliveryDate 
					&& x.Fruit.IdFruitsCatalog == Fruit.IdFruitsCatalog 
					&& x.Provider.IdProviderCatalog == Stock.Provider.IdProviderCatalog);
			}

			PriceLbl = objCatalog == null;
			PriceCatalogLbl = objCatalog != null;

			if (objCatalog != null)
			{
				Price = objCatalog.Price;
			}
		}

		#endregion

		#region Validation WPF

		[NotMapped]
		public List<string> ListHasErrorProperty { get; set; } = new();

		[NotMapped]
		public string Error { get; } = String.Empty;

		[NotMapped]
		public string this[string columnName]
		{
			get
			{
				string error = String.Empty;
				switch (columnName)
				{
					case nameof(Price):
						//Обработка ошибок для свойства Price
						if (Price == 0 || Price < 0)
						{
							error = "Сумма должна быть больше 0!";
						}
						break;
					case nameof(Mass):
						//Обработка ошибок для свойства Mass
						if (Mass == 0 || Mass < 0)
						{
							error = "Вес должен быть больше 0!";
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
