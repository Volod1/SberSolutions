using Microsoft.AspNetCore.Mvc;
using SberSolutions.Api.Models;
using System;

namespace SberSolutions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LunchController: ControllerBase
    {
        /// <summary>
        /// Метод предназначен для расчета стоимости обеда сотрудника за указанный период
        /// с учетом перерыва и изменения стоимости обеда
        /// </summary>
        /// <param name="request">Настройки обеда</param>
        /// <returns>Возвращает стоимость обеда за указанный период, с учетом входных параметров</returns>
        [HttpGet("[action]")]
        public IActionResult GetPaid([FromQuery]LunchRequest request)
        {
            if (request.Start >= request.End || request.ExcludeStart >= request.ExcludeEnd)
                return BadRequest("Дата начала не может быть больше или равна дате окончания");

            if (request.CostChangeStart < request.Start || request.CostChangeStart > request.End)
                return BadRequest("Дата смены стоимости обеда должна входить в расчетный период");

            if (request.CostChangeStart != default && request.CostAfterChange == default)
                return BadRequest("Необходимо указать последующую стоимость обеда после указанной даты");

            try
            {
                decimal paid = 0;
                double totalDays = 0;
                
                //если отсутствует дата смены стоимости обеда, то считаем весь период одинаково
                if (request.CostChangeStart == default)
                {
                    totalDays = GetTotalDaysOfPeriod(request.Start, request.End, request.ExcludeStart, request.ExcludeEnd);
                    paid = Convert.ToInt32(Math.Ceiling(totalDays)) * request.Cost;
                }
                //в случае, если установлена дата смены стоимости, то
                else
                {
                    //считаем стоимость за первый период (до даты смены стоимости обеда)
                    totalDays = GetTotalDaysOfPeriod(request.Start, request.CostChangeStart, request.ExcludeStart, request.ExcludeEnd);
                    paid = Convert.ToInt32(Math.Ceiling(totalDays)) * request.Cost;

                    //считаем стоимость за второй период (после даты смены стоимости обеда)
                    totalDays = GetTotalDaysOfPeriod(request.CostChangeStart, request.End, request.ExcludeStart, request.ExcludeEnd);
                    paid += Convert.ToInt32(Math.Ceiling(totalDays)) * request.CostAfterChange;
                }

                return Ok(paid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Метод возвращает количество дней за период
        /// </summary>
        /// <param name="start">Начало периода</param>
        /// <param name="end">Конец периода</param>
        /// <param name="excludeStart">Начало периода не входящего в расчет</param>
        /// <param name="excludeEnd">Конец периода не входящего в расчет</param>
        /// <returns>количество дней за период</returns>
        private static double GetTotalDaysOfPeriod(DateTime start, DateTime end, DateTime excludeStart, DateTime excludeEnd)
        {
            //Фильтр покрывает весь период
            if (excludeStart <= start && excludeEnd >= end)
                return 0;

            //Фильтр не попадает в диапазон установленного периода
            //считаем все дни в результат
            if (excludeStart == default || excludeEnd == default || excludeStart > end || excludeEnd < start)
                return (end - start).TotalDays;

            //Если исключенный диапазон находится внутри диапазона периода
            if (excludeStart >= start && excludeEnd <= end)
                return (excludeStart - start).TotalDays + (end - excludeEnd).TotalDays;

            //Исключенный диапазон покрывает начало установленного периода
            if (excludeEnd > start)
                return (end - excludeEnd).TotalDays;

            //Исключенный диапазон покрывает конец установленного периода
            if (excludeStart < end)
                return (excludeStart - start).TotalDays;
            
            throw new Exception("Не нашлось подходящих решений");
        }
    }
}
