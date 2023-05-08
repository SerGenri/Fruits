using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fruits.Domain.DB
{
	/// <summary>
	/// ���������
	/// </summary>
    public class ProvidersCatalog : Base.Base, IDataErrorInfo
	{
	    private string _nameProvider;
	    private ICollection<PriceCatalog> _listPriceCatalog;
	    private ICollection<Stock> _listStock;

		#pragma warning disable CS8618
		public ProvidersCatalog()
		#pragma warning restore CS8618
	    {
	        // ReSharper disable once VirtualMemberCallInConstructor
	        ListPriceCatalog = new List<PriceCatalog>();
            // ReSharper disable once VirtualMemberCallInConstructor
            ListStock = new List<Stock>();
        }

        [Key]
        public int IdProviderCatalog { get; set; }

		/// <summary>
		/// ������������ ����������
		/// </summary>
        [Required(ErrorMessage = "������� ��� ����������!")]
        public string NameProvider
        {
	        get => _nameProvider;
	        set => Set(ref _nameProvider, value);
        }

		/// <summary>
		/// ������ ������� ��������	(�����)
		/// </summary>
        public virtual ICollection<PriceCatalog> ListPriceCatalog
        {
	        get => _listPriceCatalog;
	        set => Set(ref _listPriceCatalog, value);
        }

		/// <summary>
		/// ������ ��������
		/// </summary>
        public virtual ICollection<Stock> ListStock
        {
	        get => _listStock;
	        set => Set(ref _listStock, value);
        }

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
			        case nameof(NameProvider):
						//��������� ������ ��� �������� NameProvider
						if (string.IsNullOrEmpty(NameProvider))
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
