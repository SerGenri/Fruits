using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using Fruits.Domain.Base;
using Fruits.Interfaces;

namespace Fruits.Wpf.Core.ViewModels
{
    public class ViewModelReport : Base
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<Domain.Report.Report> _listReport;

		#pragma warning disable CS8618
        public ViewModelReport(IReportServices db)
		#pragma warning restore CS8618
        {
            StartDate = DateTime.Now.Date.AddYears(-1);
            EndDate = DateTime.Now.Date.AddYears(1);

            DbReport = db;

			GetReport().ConfigureAwait(false);
        }

        /// <summary>
        /// Модель базы данных
        /// </summary>
        private IReportServices DbReport { get; set; }


        /// <summary>
        /// Дата начала отчетного периода
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set => Set(ref _startDate, value);
        }

        /// <summary>
        /// Дата конца отчетного периода
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;
            set => Set(ref _endDate, value);
        }


        private CollectionView ListReportView { get; set; }

        /// <summary>
        /// Таблица для отчета
        /// </summary>
        public ObservableCollection<Domain.Report.Report> ListReport
        {
            get => _listReport;
            set => Set(ref _listReport, value);
        }


		/// <summary>
		/// Формируем отчет
		/// </summary>
		private async Task GetReport()
		{
			var list = await DbReport.GetReportAsync(StartDate, EndDate);
			ListReport = new ObservableCollection<Domain.Report.Report>(list);

			// Группировка
			ListReportView = (CollectionView)CollectionViewSource.GetDefaultView(ListReport);
			PropertyGroupDescription groupDescription = new PropertyGroupDescription(nameof(Domain.Report.Report.NameProvider));
			ListReportView.GroupDescriptions?.Add(groupDescription);

			ListReportView.Refresh();
		}


		/// <summary>
		/// Запуск выгрузки отчета
		/// </summary>
		private RelayCommand _reportCommand;
        public RelayCommand ReportCommand
        {
            get
            {
                return _reportCommand ??= new RelayCommand(async obj =>
                {
                    await GetReport();

                });
            }
        }
    }
}