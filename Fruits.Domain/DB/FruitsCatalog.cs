using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// Фрукт
	/// </summary>
    public class FruitsCatalog : Base.Base, IDataErrorInfo
	{
	    private string _class;
	    private string _sort;
	    private ICollection<PriceCatalog> _listPriceCatalog;
	    private ICollection<StockFruits> _listStockFruits;

		#pragma warning disable CS8618
	    public FruitsCatalog()
		#pragma warning restore CS8618
	    {
	        // ReSharper disable once VirtualMemberCallInConstructor
	        ListPriceCatalog = new List<PriceCatalog>();
	        // ReSharper disable once VirtualMemberCallInConstructor
	        ListStockFruits = new List<StockFruits>();
        }

        [Key]
        public int IdFruitsCatalog { get; set; }

		/// <summary>
		/// Вид фрукта
		/// </summary>
        [Required(ErrorMessage = "Введите вид фрукта!")]
        public string Class
        {
	        get => _class;
	        set => Set(ref _class, value);
        }

		/// <summary>
		/// Сорт фрукта
		/// </summary>
        [Required(ErrorMessage = "Введите сорт фрукта!")]
        public string Sort
        {
	        get => _sort;
	        set => Set(ref _sort, value);
        }

		/// <summary>
		/// Список Графика поставок
		/// </summary>
        public virtual ICollection<PriceCatalog> ListPriceCatalog
        {
	        get => _listPriceCatalog;
	        set => Set(ref _listPriceCatalog, value);
        }

		/// <summary>
		/// Список Комплекта поставок
		/// </summary>
        public virtual ICollection<StockFruits> ListStockFruits
        {
	        get => _listStockFruits;
	        set => Set(ref _listStockFruits, value);
        }

		#region NotMapped

		[NotMapped]
		public string FullName => $"{Class} - {Sort}".Replace("--- Выбери - ---", "--- Выбери ---");

		#endregion

		#region Validation WPF
		[NotMapped]
		public List<string> ListHasErrorProperty { get; set; } = new();

		[NotMapped]
		public string Error { get;} = String.Empty;

		[NotMapped]
		public string this[string columnName]
		{
			get
			{
				string error = String.Empty;
				switch (columnName)
				{
					case nameof(Class):
						//Обработка ошибок для свойства Class
						if (string.IsNullOrEmpty(Class))
						{
							error = "Поле не должно быть пустым!";
						}
						break;
					case nameof(Sort):
						//Обработка ошибок для свойства Sort
						if (string.IsNullOrEmpty(Sort))
						{
							error = "Поле не должно быть пустым!";
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
