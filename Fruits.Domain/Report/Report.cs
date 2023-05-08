namespace Fruits.Domain.Report
{
    /// <summary>
    /// Отчет по поставкам фруктов
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Имя провайдера
        /// </summary>
        public string? NameProvider { get; set; }

        /// <summary>
        /// Имя фрукта
        /// </summary>
        public string? NameFruit { get; set; }

        /// <summary>
        /// Суммарная масса по всем поставкам
        /// </summary>
        public int MassSumm { get; set; }

        /// <summary>
        /// Суммарная цена по всем поставкам
        /// </summary>
        public double PriceSumm { get; set; }

    }
}