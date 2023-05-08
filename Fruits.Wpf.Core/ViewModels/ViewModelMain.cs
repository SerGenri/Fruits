using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Fruits.Domain.Base;
using Fruits.Interfaces;
using Fruits.Wpf.Core.Catalog.FruitsCatalog;
using Fruits.Wpf.Core.Catalog.PriceCatalog;
using Fruits.Wpf.Core.Catalog.ProvidersCatalog;
using Fruits.Wpf.Core.Report;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FruitsCatalog = Fruits.Domain.DB.FruitsCatalog;
using ProvidersCatalog = Fruits.Domain.DB.ProvidersCatalog;
using Stock = Fruits.Domain.DB.Stock;
using StockFruits = Fruits.Domain.DB.StockFruits;

namespace Fruits.Wpf.Core.ViewModels
{
    public class ViewModelMain : Base
    {
	    private Stock? _listStockEntry;
        private ProvidersCatalog _comboBoxListProvidersCatalogEntry;
        private FruitsCatalog? _comboBoxListFruitsCatalogEntry;
        private StockFruits? _listStockFruitsEntry;
        private bool _formStockLock;
        private bool _formStockFruitsPropLock;
        private bool _priceCatalogPropLock;
        private bool _formStockPropLock;
        private ObservableCollection<Stock>? _listStock;
        private ObservableCollection<ProvidersCatalog> _comboBoxListProvidersCatalog;
        private ICollection<StockFruits>? _listStockFruits;
        private ObservableCollection<FruitsCatalog> _comboBoxListFruitsCatalog;

        /// <summary>
        /// Ловим все необработанные ошибки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show("HResult : " + e.HResult + Environment.NewLine + Environment.NewLine +
                            "InnerException : " + e.InnerException + Environment.NewLine + Environment.NewLine +
                            "Data : " + e.Data + Environment.NewLine + Environment.NewLine +
                            "Source : " + e.Source + Environment.NewLine + Environment.NewLine +
                            "TargetSite : " + e.TargetSite + Environment.NewLine + Environment.NewLine +
                            "Message : " + e.Message);
        }


		#pragma warning disable CS8618
        public ViewModelMain(IFriutServices db)
		#pragma warning restore CS8618
        {
	        //Глобальные исключений в приложении
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MainHandler;

            //для установки формата даты и времени по системный параметрам
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));

            DbMain = db;

			InitializeDb();
        }

		/// <summary>
		/// Инициализация БД
		/// </summary>
		private async void InitializeDb()
		{
            // Настройка файла конфигурации
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.SetBasePath(App.CurrentDirectory);
			configurationBuilder.AddJsonFile("appsettings.json", true, true);
			IConfigurationRoot configuration = configurationBuilder.Build();

	        // Читаем параметр из файла конфигурации
	        // Генерить тестовые данные
	        var initDb = true;
	        var generateData = configuration.GetSection("DataBase").GetSection("GenerateData").Value;

	        bool.TryParse(generateData, out initDb);
	        // Читаем параметр из файла конфигурации
	        // Удалить все данные в базе
	        var removeBefore = false;
	        var removeData = configuration.GetSection("DataBase").GetSection("RemoveData").Value;
	        bool.TryParse(removeData, out removeBefore);

			var scop = App.Host.Services.CreateAsyncScope();

	        var dbInitializer = scop.ServiceProvider.GetRequiredService<IDbInitializer>();
	        await dbInitializer.InitializeAsync(removeBefore, initDb);

	        Load();
        }


		/// <summary>
		/// Выгрузка информации из базы
		/// </summary>
		private async void Load()
        {
	        try
	        {
		        var idStock = ListStockEntry?.IdStock;
		        ListStockEntry = null;

				var stock = await DbMain.GetAllStock();
		        ListStock = new ObservableCollection<Stock>(stock);
				ListStockView = (CollectionView)CollectionViewSource.GetDefaultView(ListStock);
    
				//==================== ComboBox
				var providers = await DbMain.GetAllProvidersCatalog();
		        ComboBoxListProvidersCatalog = new ObservableCollection<ProvidersCatalog>(providers);

		        var fruits = await DbMain.GetAllFruitsCatalog();
		        ComboBoxListFruitsCatalog = new ObservableCollection<FruitsCatalog>(fruits);

		        if (ListStock.Any()) ListStockEntry = ListStock.FirstOrDefault(x => x.IdStock == idStock);

		        // Подписка на события изменения полей
		        foreach (var stockItem in ListStock)
		        {
			        stockItem.PropertyChanged += ItemOnPropertyChanged;

			        foreach (var itemStockFruit in stockItem.ListStockFruits)
			        {
				        itemStockFruit.PropertyChanged += ItemOnPropertyChanged;
			        }
				}

		        FormStockLock = true;
			}
			catch (Exception e)
	        {
		        MessageBox.Show(e.Message);
	        }
        }

        /// <summary>
        /// Записываем изменения в базу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
	        if (sender is Stock stock 
	            && (e.PropertyName == nameof(Stock.DeliveryDate) 
	                || e.PropertyName == nameof(Stock.Provider)))
	        {
		        DbMain.UpdateStock(stock);
			}

	        if (sender is StockFruits stockFruits
				&& (e.PropertyName == nameof(StockFruits.Price)
	                || e.PropertyName == nameof(StockFruits.Mass) 
	                || e.PropertyName == nameof(StockFruits.Fruit)))
	        {
		        if (stockFruits.IdStockFruits > 0)
		        {
			        DbMain.UpdateStockFruits(stockFruits);
				}

				ListStockView?.Refresh();
			}
		}


        /// <summary>
		/// Модель базы данных
		/// </summary>
		private IFriutServices DbMain { get; set; }

        #region ======================== Stock ========================

        /// <summary>
        /// Блокировка полей для редактирования таблицы Поставщики
        /// </summary>
        public bool FormStockPropLock
        {
            get => _formStockPropLock;
            set => Set(ref _formStockPropLock, value);
        }

        private CollectionView? ListStockView { get; set; }

		/// <summary>
		/// Таблица Поставок
		/// </summary>
		public ObservableCollection<Stock>? ListStock
        {
            get => _listStock;
            set => Set(ref _listStock, value);
        }

        /// <summary>
        /// Выбранный пункт из таблицы поставок
        /// </summary>
        public Stock? ListStockEntry
        {
            get => _listStockEntry;
            set
            {
	            if (Set(ref _listStockEntry, value))
	            {
                     var list = _listStockEntry?.ListStockFruits;
		            if (list != null)
					{
						ListStockFruits = list;
						ListStockFruitsView = (CollectionView)CollectionViewSource.GetDefaultView(ListStockFruits);

						ListStockFruitsView?.SortDescriptions.Add(new SortDescription(nameof(StockFruits.FullName), ListSortDirection.Ascending));
						ListStockFruitsView?.SortDescriptions.Add(new SortDescription(nameof(StockFruits.Mass), ListSortDirection.Ascending));
						ListStockFruitsView?.SortDescriptions.Add(new SortDescription(nameof(StockFruits.Price), ListSortDirection.Ascending));
					}

					ComboBoxListProvidersCatalogEntry = value?.Provider ?? ComboBoxListProvidersCatalog.First();

		            FormStockPropLock = value != null;

		            OnPropertyChanged(nameof(ListStockFruits));
				}
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

                if (ListStockEntry != null)
                {
                    ListStockEntry.Provider = value;
                    ListStockFruitsView?.Refresh();
                }
            }
        }


        /// <summary>
        /// Добавить пункт в таблицу поставок
        /// </summary>
        private RelayCommand _addStockCommand;
        public RelayCommand AddStockCommand
        {
            get
            {
                return _addStockCommand ??= new RelayCommand(obj =>
                {
	                var provider = ComboBoxListProvidersCatalog.FirstOrDefault();

					if (provider == null)
					{
						MessageBox.Show("Список поставщиков пуст, добавьте хотя бы один!");
                          return;
					}

                    Stock objStock = new Stock
                    {
                        DeliveryDate = DateTime.Now.Date,
                        Provider = provider
					};

                    ListStock?.Insert(0, objStock);
                    DbMain.AddStockAsync(objStock);

                    ListStockEntry = objStock;
                });
            }
        }

        /// <summary>
        /// Удалить выбранный пункт из таблицы поставок
        /// </summary>
        private RelayCommand _removeStockCommand;
        public RelayCommand RemoveStockCommand
        {
            get
            {
                return _removeStockCommand ??= new RelayCommand(async obj =>
                {
                    if (ListStockEntry?.ListStockFruits != null)
                    {
                        await DbMain.RemoveStockAsync(ListStockEntry);
						ListStock?.Remove(ListStockEntry);
                    }
                }, obj => ListStockEntry != null);
            }
        }

        #endregion ======================== Stock ========================

        #region ======================== StockFruits ========================

        /// <summary>
        /// Блокировка полей для редактирования таблицы фрукты
        /// </summary>
        public bool FormStockFruitsPropLock
        {
            get => _formStockFruitsPropLock;
            set => Set(ref _formStockFruitsPropLock, value);
        }

        /// <summary>
        /// Блокировка поля Сумма
        /// </summary>
        public bool PriceCatalogPropLock
        {
            get => _priceCatalogPropLock;
            set => Set(ref _priceCatalogPropLock, value);
        }


        /// <summary>
        /// Блокировка таблицы поставщики
        /// </summary>
        public bool FormStockLock
        {
            get => _formStockLock;
            set => Set(ref _formStockLock, value);
        }

        private void GetFormStockLock()
        {
            if (ListStockFruits != null && ListStockFruits.Any())
            {
                FormStockLock = !ListStockFruits.Any(x => x.FullName != null && x.FullName.Contains("--- Выбери"));
            }
        }



        private CollectionView? ListStockFruitsView { get; set; }

        /// <summary>
        /// Таблица с фруктами, комплект  поставки 
        /// </summary>
        public ICollection<StockFruits>? ListStockFruits
        {
            get => _listStockFruits;
            set => Set(ref _listStockFruits, value);
        }

        /// <summary>
        /// Выбранный в данный момент пункт таблицы фрукты
        /// </summary>
        public StockFruits? ListStockFruitsEntry
        {
            get => _listStockFruitsEntry;
            set
            {
                Set(ref _listStockFruitsEntry, value);

                ComboBoxListFruitsCatalogEntry = value?.Fruit ?? ComboBoxListFruitsCatalog.FirstOrDefault();
                FormStockFruitsPropLock = value != null;
            }
        }


        /// <summary>
        /// Список фруктов
        /// </summary>
        public ObservableCollection<FruitsCatalog> ComboBoxListFruitsCatalog
        {
            get => _comboBoxListFruitsCatalog;
            set => Set(ref _comboBoxListFruitsCatalog, value);
        }

        /// <summary>
        /// Выбранный фрукт в комбобоксе фрукты
        /// </summary>
        public FruitsCatalog? ComboBoxListFruitsCatalogEntry
        {
            get => _comboBoxListFruitsCatalogEntry;
            set
            {
                Set(ref _comboBoxListFruitsCatalogEntry, value);

                if (ListStockFruitsEntry != null)
                {
                    ListStockFruitsEntry.Fruit = value ?? new FruitsCatalog();

                    PriceCatalogPropLock = ListStockFruitsEntry.PriceLbl;
                }

                GetFormStockLock();
            }
        }




        /// <summary>
        /// Добавить пункт в таблицу фрукты
        /// </summary>
        private RelayCommand _addStockFruitsCommand;
        public RelayCommand AddStockFruitsCommand
        {
            get
            {
                return _addStockFruitsCommand ??= new RelayCommand(async obj =>
                {
	                var fruits = ComboBoxListFruitsCatalog.FirstOrDefault();
	                if (fruits == null)
	                {
		                MessageBox.Show("Список фруктов пуст, добавьте хотя бы один!");
		                return;
	                }

	                StockFruits objStockFruits = new StockFruits();
	                objStockFruits.Stock = ListStockEntry ?? new Stock();
	                objStockFruits.Fruit = fruits;

	                objStockFruits.PropertyChanged += ItemOnPropertyChanged;

					ListStockFruits?.Add(objStockFruits);
                    await DbMain.AddStockFruitsAsync(objStockFruits);

                    GetFormStockLock();

                    ListStockFruitsEntry = objStockFruits;

                    ListStockView?.Refresh();
					ListStockFruitsView?.Refresh();


                }, obj => ListStockEntry != null);
            }
        }


        /// <summary>
        /// Удалить выбранный пункт из таблицы фрукты 
        /// </summary>
        private RelayCommand _removeStockFruitsCommand;
        public RelayCommand RemoveStockFruitsCommand
        {
            get
            {
                return _removeStockFruitsCommand ??= new RelayCommand(async obj =>
                {
                    if (ListStockFruitsEntry != null)
					{
						ListStockFruits?.Remove(ListStockFruitsEntry);
						await DbMain.RemoveStockFruitsAsync(ListStockFruitsEntry);
                    }

                    GetFormStockLock();

                    ListStockView?.Refresh();
					ListStockFruitsView?.Refresh();

                }, obj => ListStockFruitsEntry != null);
            }
        }


        #endregion ======================== StockFruits ========================


        /// <summary>
        /// Выгрузить данные, без сохранения изменений
        /// </summary>
        private RelayCommand _loadStockCommand;
        public RelayCommand LoadStockCommand
        {
            get
            {
                return _loadStockCommand ??= new RelayCommand(obj =>
                {
                    Load();

                });
            }
        }


		/// <summary>
		/// Сохранить сделанные изменения в базу
		/// </summary>
		private RelayCommand _saveStockCommand;
        public RelayCommand SaveStockCommand
        {
            get
            {
                return _saveStockCommand ??= new RelayCommand(async obj =>
                {
	                try
	                {
		                await DbMain.SaveChangesAsync();
	                }
	                catch (Exception e)
	                {
		                MessageBox.Show(e.Message);
	                }

					Load();

                }, obj => ListStock != null && !ListStock.Any(x=>x.ListStockFruits.Any(y => y.ListHasErrorProperty.Count > 0)));
            }
        }


        //======================== Справочники


        /// <summary>
        /// Открыть справочник Фрукты
        /// </summary>
        private RelayCommand _fruitsCatalogCommand;
        public RelayCommand FruitsCatalogCommand
        {
            get
            {
                return _fruitsCatalogCommand ??= new RelayCommand(obj =>
                {
                    WindowFruitsCatalog form = new WindowFruitsCatalog();
                    form.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Открыть справочник Поставщики
        /// </summary>
        private RelayCommand _providersCatalogCommand;
        public RelayCommand ProvidersCatalogCommand
        {
            get
            {
                return _providersCatalogCommand ??= new RelayCommand(obj =>
                {
                    WindowProvidersCatalog form = new WindowProvidersCatalog();
                    form.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Открыть справочник График поставок
        /// </summary>
        private RelayCommand _priceCatalogCommand;
        public RelayCommand PriceCatalogCommand
        {
            get
            {
                return _priceCatalogCommand ??= new RelayCommand(obj =>
                {
                    WindowPriceCatalog form = new WindowPriceCatalog();
                    form.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Открыть отчет
        /// </summary>
        private RelayCommand _reportCommand;
        public RelayCommand ReportCommand
        {
            get
            {
                return _reportCommand ??= new RelayCommand(obj =>
                {
                    WindowReport form = new WindowReport();
                    form.ShowDialog();
                });
            }
        }
    }
}