using Microsoft.AspNetCore.Mvc;
using SberSolutions.Api.Models;
using SberSolutions.Api.Services;
using System;

namespace SberSolutions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LunchController: ControllerBase
    {
        private ILunchService _lunchService;

        public LunchController(ILunchService lunchService)
        {
            _lunchService = lunchService;
        }

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
                return Ok(_lunchService.GetPaid(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
