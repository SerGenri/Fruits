using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fruits.Domain.Base;
using Fruits.Domain.DB;
using Fruits.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fruits.Wpf.Core.ViewModels
{
    public class ViewModelPriceCatalog : Base
    {
        private ProvidersCatalog _comboBoxListProvidersCatalogEntry;
        private FruitsCatalog _comboBoxListFruitsCatalogEntry;
        private PriceCatalog? _listPriceCatalogEntry;
        private bool _boolListPriceCatalogEntry;
        private ObservableCollection<PriceCatalog>? _listPriceCatalog;
        private ObservableCollection<ProvidersCatalog> _comboBoxListProvidersCatalog;
        private ObservableCollection<FruitsCatalog> _comboBoxListFruitsCatalog;

		#pragma warning disable CS8618
        public ViewModelPriceCatalog(IFriutServices db)
		#pragma warning restore CS8618
		{
			DbPriceCatalog = db;

			Load();
        }


        /// <summary>
        /// Выгрузка данных
        /// </summary>
        private async void Load()
        {
			var price = await DbPriceCatalog.GetAllPriceCatalog();
            ListPriceCatalog = new ObservableCollection<PriceCatalog>(price);

            //==================== ComboBox
            var providers = await DbPriceCatalog.GetAllProvidersCatalog();
            ComboBoxListProvidersCatalog = new ObservableCollection<ProvidersCatalog>(providers);
            ComboBoxListProvidersCatalog = new ObservableCollection<ProvidersCatalog>(ComboBoxListProvidersCatalog.OrderBy(x => x.NameProvider));

            var fruits = await DbPriceCatalog.GetAllFruitsCatalog();
            ComboBoxListFruitsCatalog = new ObservableCollection<FruitsCatalog>(fruits);
            ComboBoxListFruitsCatalog = new ObservableCollection<FruitsCatalog>(ComboBoxListFruitsCatalog.OrderBy(x => x.Class).ThenBy(x => x.Sort));
        }

        /// <summary>
        /// Модель базы данных
        /// </summary>
        private IFriutServices DbPriceCatalog { get; set; }

        /// <summary>
        /// Блокировка болей если не выбран элемент в таблице план поставок
        /// </summary>
        public bool BoolListPriceCatalogEntry
        {
            get => _boolListPriceCatalogEntry;
            set => Set(ref _boolListPriceCatalogEntry, value);
        }


        /// <summary>
        /// Таблица план поставок
        /// </summary>
        public ObservableCollection<PriceCatalog>? ListPriceCatalog
        {
            get => _listPriceCatalog;
            set => Set(ref _listPriceCatalog, value);
        }

        /// <summary>
        /// Выбранная строка в таблице план поставок
        /// </summary>
        public PriceCatalog? ListPriceCatalogEntry
        {
            get => _listPriceCatalogEntry;
            set
            {
                Set(ref _listPriceCatalogEntry, value);

                ComboBoxListFruitsCatalogEntry = value?.Fruit ?? ComboBoxListFruitsCatalog.First();
                ComboBoxListProvidersCatalogEntry = value?.Provider ?? ComboBoxListProvidersCatalog.First();

                BoolListPriceCatalogEntry = value != null;
            }
        }


        /// <summary>
        /// Список Поставщиков
        /// </summary>
        public ObservableCollection<ProvidersCatalog> ComboBoxListProvidersCatalog
        {
            get => _comboBoxListProvidersCatalog;
            set => Set(ref _comboBoxListProvidersCatalog, value);
        }

        /// <summary>
        /// Выбранный поставщик в комбобоксе поставщики
        /// </summary>
        public ProvidersCatalog ComboBoxListProvidersCatalogEntry
        {
            get => _comboBoxListProvidersCatalogEntry;
            set
            {
                Set(ref _comboBoxListProvidersCatalogEntry, value);

                if (ListPriceCatalogEntry != null)
                {
                    ListPriceCatalogEntry.Provider = value;
                }

                GetFormLock();
            }
        }

        /// <summary>
        /// Список Фруктов
        /// </summary>
        public ObservableCollection<FruitsCatalog> ComboBoxListFruitsCatalog
        {
            get => _comboBoxListFruitsCatalog;
            set => Set(ref _comboBoxListFruitsCatalog, value);
        }

        /// <summary>
        /// Выбранный поставщик в комбобоксе фрукты
        /// </summary>
        public FruitsCatalog ComboBoxListFruitsCatalogEntry
        {
            get => _comboBoxListFruitsCatalogEntry;
            set
            {
                Set(ref _comboBoxListFruitsCatalogEntry, value);

                if (ListPriceCatalogEntry != null)
                {
                    ListPriceCatalogEntry.Fruit = value;
                }

                GetFormLock();
            }
        }



        /// <summary>
        /// Блокировка кнопки сохранить если не выбран поставщик или вид фруктов
        /// </summary>
        private bool SaveDbLock { get; set; }

        private void GetFormLock()
        {
            if (ListPriceCatalog != null && ListPriceCatalog.Any())
            {
                SaveDbLock = !ListPriceCatalog.Any(x => (x.Provider.NameProvider.Contains("--- Выбери ---")
                                                         || x.Fruit.FullName.Contains("--- Выбери ---")));
            }
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
	                var fruits = ComboBoxListFruitsCatalog.FirstOrDefault();
	                var providers = ComboBoxListProvidersCatalog.FirstOrDefault();

	                if (fruits == null && providers == null)
	                {
		                MessageBox.Show("Список поставщиков и фруктов пуст, добавьте хотя бы один!");
		                return;
					}
					if (fruits == null)
	                {
		                MessageBox.Show("Список фруктов пуст, добавьте хотя бы один!");
                        return;
	                }
	                if (providers == null)
	                {
		                MessageBox.Show("Список поставщиков пуст, добавьте хотя бы одного!");
                        return;
	                }

					PriceCatalog objPriceCatalog = new PriceCatalog(fruits, providers);

                    DbPriceCatalog.AddPriceCatalogAsync(objPriceCatalog);
					ListPriceCatalog?.Insert(0, objPriceCatalog);

                    ListPriceCatalogEntry = objPriceCatalog;

                    GetFormLock();

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
                return _removeCommand ??= new RelayCommand(obj =>
                {
	                if (ListPriceCatalogEntry != null)
	                {
		                DbPriceCatalog.RemovePriceCatalogAsync(ListPriceCatalogEntry);
		                ListPriceCatalog?.Remove(ListPriceCatalogEntry);
	                }

                    GetFormLock();

                }, obj => ListPriceCatalogEntry != null);
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
	                await DbPriceCatalog.SaveChangesAsync();
					Load();

                }, obj => SaveDbLock && ListPriceCatalog != null && !ListPriceCatalog.Any(x => x.ListHasErrorProperty.Count > 0));
            }
        }



    }

}