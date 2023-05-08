using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// ��������
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
		/// ���� ��������
		/// </summary>
        [Required, Range(typeof(DateTime), "2000-01-01", "2099-01-01", ErrorMessage = "���� ������ ���� � ��������� �� {1} �� {2}")]
		public DateTime DeliveryDate
        {
	        get => _deliveryDate;
	        set => Set(ref _deliveryDate, value);
        }

		[Range(1, int.MaxValue, ErrorMessage = "�������� ����������!")]
        public int IdProviderCatalog
        {
	        get => _idProviderCatalog;
	        set => Set(ref _idProviderCatalog, value);
        }

		/// <summary>
		/// ���������
		/// </summary>
		[ForeignKey(nameof(IdProviderCatalog))]
		public virtual ProvidersCatalog Provider
        {
	        get => _provider;
	        set => Set(ref _provider, value);
        }

		/// <summary>
		/// ������ ��������� ��������
		/// </summary>
		public virtual ICollection<StockFruits> ListStockFruits
        {
	        get => _listStockFruits;
	        set => Set(ref _listStockFruits, value);
        }

		#region NotMapped
		/// <summary>
		/// ����� ���� ��������
		/// </summary>
		[NotMapped]
		public double PriceSumm => ListStockFruits.Sum(x => x.Mass * x.Price);

		/// <summary>
		/// ����� ���� ��������
		/// </summary>
        [NotMapped]
		public int MassSumm => ListStockFruits.Sum(x => x.Mass);
        #endregion
	}
}
