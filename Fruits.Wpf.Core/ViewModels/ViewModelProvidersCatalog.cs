using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fruits.Domain.Base;
using Fruits.Domain.DB;
using Fruits.Interfaces;

namespace Fruits.Wpf.Core.ViewModels
{
    public class ViewModelProvidersCatalog : Base
    {

        private ProvidersCatalog? _listProvidersCatalogEntry;
        private ObservableCollection<ProvidersCatalog>? _listProvidersCatalog;
        private bool _boolListProvidersCatalogEntry;

		#pragma warning disable CS8618
        public ViewModelProvidersCatalog(IFriutServices db)
		#pragma warning restore CS8618
        {
	        DbProvidersCatalog  = db;

			Load();
        }

        /// <summary>
        /// Выгрузка данных
        /// </summary>
        private async void Load()
        {
			var providers = await DbProvidersCatalog.GetAllProvidersCatalog();
            ListProvidersCatalog = new ObservableCollection<ProvidersCatalog>(providers);
        }

        /// <summary>
        /// Модель базы данных
        /// </summary>
        private IFriutServices DbProvidersCatalog { get; set; }

        /// <summary>
        /// Таблица поставщики
        /// </summary>
        public ObservableCollection<ProvidersCatalog>? ListProvidersCatalog
        {
            get => _listProvidersCatalog;
            set => Set(ref _listProvidersCatalog, value);
        }

        /// <summary>
        /// Выбранная строка в таблице поставщики
        /// </summary>
        public ProvidersCatalog? ListProvidersCatalogEntry
        {
            get => _listProvidersCatalogEntry;
            set
            {
                Set(ref _listProvidersCatalogEntry, value);

                BoolListProvidersCatalogEntry = value != null;
            }
        }

        /// <summary>
        /// Блокировка полей если не выбран элемент в таблице поставщики
        /// </summary>
        public bool BoolListProvidersCatalogEntry
        {
            get => _boolListProvidersCatalogEntry;
            set => Set(ref _boolListProvidersCatalogEntry, value);
        }

        /// <summary>
        /// Выгрузить данные, без сохранения изменений
        /// </summary>
        private RelayCommand _loadCommand;
        public RelayCommand LoadCommand
        {
	        get
	        {
		        return _loadCommand ??= new RelayCommand(obj =>
		        {
			        Load();
		        });
	        }
        }

		/// <summary>
		/// Добавить строку
		/// </summary>
		private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RelayCommand(obj =>
                {
	                ProvidersCatalog objProvidersCatalog = new ProvidersCatalog();
	                objProvidersCatalog.NameProvider = "Введите имя поставщика!";

					DbProvidersCatalog.AddProvidersCatalogAsync(objProvidersCatalog);
					ListProvidersCatalog?.Insert(0, objProvidersCatalog);
                    
					ListProvidersCatalogEntry = ListProvidersCatalog?.First();

                });
            }
        }

        /// <summary>
        /// Удалить строку
        /// </summary>
        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??= new RelayCommand(async obj =>
                {
	                if (ListProvidersCatalogEntry != null)
					{
						var result = await DbProvidersCatalog.RemoveProvidersCatalogAsync(ListProvidersCatalogEntry);
						if (result.Result)
						{
							ListProvidersCatalog?.Remove(ListProvidersCatalogEntry);
						}
						else
						{
							MessageBox.Show(result.ErrorMessage, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}

				}, obj => ListProvidersCatalogEntry != null);
            }
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(async obj =>
                {
	                await DbProvidersCatalog.SaveChangesAsync();
					Load();

                }, obj => ListProvidersCatalog != null && !ListProvidersCatalog.Any(x => x.ListHasErrorProperty.Count > 0));
            }
        }

    }
}