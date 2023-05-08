using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// Поставка
	/// </summary>
	public class Stock : Base.Base
	{
	    private DateTime _deliveryDate;
	    private ProvidersCatalog _provider;
	    private ICollection<StockFruits> _listStockFruits;
	    private int _idProviderCatalog;

		#pragma warning disable CS8618
	    public Stock()
		#pragma warning restore CS8618
	    {
	        // ReSharper disable once VirtualMemberCallInConstructor
	        ListStockFruits = new List<StockFruits>();
	        // ReSharper disable once VirtualMemberCallInConstructor
	        Provider = new ProvidersCatalog();
	    }

        [Key]
        public int IdStock { get; set; }

		/// <summary>
		/// Дата поставки
		/// </summary>
        [Required, Range(typeof(DateTime), "2000-01-01", "2099-01-01", ErrorMessage = "Дата должна быть в диапазоне от {1} до {2}")]
		public DateTime DeliveryDate
        {
	        get => _deliveryDate;
	        set => Set(ref _deliveryDate, value);
        }

		[Range(1, int.MaxValue, ErrorMessage = "Выберите поставщика!")]
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

		/// <summary>
		/// Список Комплекта поставки
		/// </summary>
		public virtual ICollection<StockFruits> ListStockFruits
        {
	        get => _listStockFruits;
	        set => Set(ref _listStockFruits, value);
        }

		#region NotMapped
		/// <summary>
		/// Сумма всей поставки
		/// </summary>
		[NotMapped]
		public double PriceSumm => ListStockFruits.Sum(x => x.Mass * x.Price);

		/// <summary>
		/// Сумма веса поставки
		/// </summary>
        [NotMapped]
		public int MassSumm => ListStockFruits.Sum(x => x.Mass);
        #endregion
	}
}
