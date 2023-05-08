using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fruits.Domain.Base;
using Fruits.Domain.DB;
using Fruits.Interfaces;

namespace Fruits.Wpf.Core.ViewModels
{
    public class ViewModelFruitsCatalog : Base
    {
        private FruitsCatalog? _listFruitsCatalogEntry;
        private ObservableCollection<FruitsCatalog>? _listFruitsCatalog;
        private bool _boolListFruitsCatalogEntry;

		#pragma warning disable CS8618
        public ViewModelFruitsCatalog(IFriutServices db)
		#pragma warning restore CS8618
        {
	        DbFruitsCatalog = db;
			Load();
        }

        /// <summary>
        /// Выгрузка данных
        /// </summary>
        private async void Load()
		{
			var fruits = await DbFruitsCatalog.GetAllFruitsCatalog();
            ListFruitsCatalog = new ObservableCollection<FruitsCatalog>(fruits);
        }

        /// <summary>
        /// Модель базы данных
        /// </summary>
        private IFriutServices DbFruitsCatalog { get; set; }


        /// <summary>
        /// Таблица фрукты
        /// </summary>
        public ObservableCollection<FruitsCatalog>? ListFruitsCatalog
        {
            get => _listFruitsCatalog;
            set => Set(ref _listFruitsCatalog, value);
        }

        /// <summary>
        /// Выбранная строка в таблице фрукты
        /// </summary>
        public FruitsCatalog? ListFruitsCatalogEntry
        {
            get => _listFruitsCatalogEntry;
            set
            {
                Set(ref _listFruitsCatalogEntry, value);
                BoolListFruitsCatalogEntry = value != null;
            }
        }


        /// <summary>
        /// Блокировка полей если не выбран элемент в таблице фрукты
        /// </summary>
        public bool BoolListFruitsCatalogEntry
        {
            get => _boolListFruitsCatalogEntry;
            set => Set(ref _boolListFruitsCatalogEntry, value);
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
					FruitsCatalog objPriceCatalog = new FruitsCatalog();
					objPriceCatalog.Class = "Введите вид фрукта!";
					objPriceCatalog.Sort = "Введите сорт фрукта!";

					DbFruitsCatalog.AddFruitsCatalogAsync(objPriceCatalog);
					ListFruitsCatalog?.Insert(0, objPriceCatalog);

                    ListFruitsCatalogEntry = ListFruitsCatalog?.FirstOrDefault();

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
	                if (ListFruitsCatalogEntry != null)
	                {
		                var result = await DbFruitsCatalog.RemoveFruitsCatalogAsync(ListFruitsCatalogEntry);
						if (result.Result)
						{
							ListFruitsCatalog?.Remove(ListFruitsCatalogEntry);
						}
						else
						{
							MessageBox.Show(result.ErrorMessage, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
						}
	                }

                }, obj => ListFruitsCatalogEntry != null);
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
	                await DbFruitsCatalog.SaveChangesAsync();
					Load();

                }, obj => ListFruitsCatalog != null && !ListFruitsCatalog.Any(x=> x.ListHasErrorProperty.Count > 0));
            }
        }

    }
}