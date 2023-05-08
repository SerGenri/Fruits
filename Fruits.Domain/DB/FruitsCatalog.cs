using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// �����
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
		/// ��� ������
		/// </summary>
        [Required(ErrorMessage = "������� ��� ������!")]
        public string Class
        {
	        get => _class;
	        set => Set(ref _class, value);
        }

		/// <summary>
		/// ���� ������
		/// </summary>
        [Required(ErrorMessage = "������� ���� ������!")]
        public string Sort
        {
	        get => _sort;
	        set => Set(ref _sort, value);
        }

		/// <summary>
		/// ������ ������� ��������
		/// </summary>
        public virtual ICollection<PriceCatalog> ListPriceCatalog
        {
	        get => _listPriceCatalog;
	        set => Set(ref _listPriceCatalog, value);
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

		[NotMapped]
		public string FullName => $"{Class} - {Sort}".Replace("--- ������ - ---", "--- ������ ---");

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
						//��������� ������ ��� �������� Class
						if (string.IsNullOrEmpty(Class))
						{
							error = "���� �� ������ ���� ������!";
						}
						break;
					case nameof(Sort):
						//��������� ������ ��� �������� Sort
						if (string.IsNullOrEmpty(Sort))
						{
							error = "���� �� ������ ���� ������!";
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
