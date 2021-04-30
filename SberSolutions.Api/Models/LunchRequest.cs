using System;
using System.ComponentModel.DataAnnotations;

namespace SberSolutions.Api.Models
{
    public class LunchRequest
    {
        /// <summary>
        /// Начало рассчетного периода
        /// </summary>
        /// <example>2021-04-03</example>
        [Required]
        public DateTime Start { get; set; }

        /// <summary>
        /// Конец рассчетного периода
        /// </summary>
        /// <example>2021-05-01</example>
        [Required]
        public DateTime End { get; set; }

        /// <summary>
        /// Начало периода не входящего в рассчет
        /// </summary>
        /// <example>2021-04-06</example>
        public DateTime ExcludeStart { get; set; }

        /// <summary>
        /// Конец периода не входящего в рассчет
        /// </summary>
        /// <example>2021-04-13</example>
        public DateTime ExcludeEnd { get; set; }

        /// <summary>
        /// Начальная стоимость обеда
        /// </summary>
        /// <example>100</example>
        [Required]
        [Range(1, Int32.MaxValue)]
        public decimal Cost { get; set; }

        /// <summary>
        /// Дата смены стоимости обеда
        /// </summary>
        /// <example>2021-04-20</example>
        public DateTime CostChangeStart { get; set; }

        /// <summary>
        /// Стоимость обеда после смены
        /// </summary>
        /// <example>300</example>
        [Range(1, Int32.MaxValue)]
        public decimal CostAfterChange { get; set; }
    }
}
