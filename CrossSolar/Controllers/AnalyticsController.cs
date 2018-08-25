using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossSolar.Controllers
{
    [Route("panel")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        private readonly IPanelRepository _panelRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository, IPanelRepository panelRepository)
        {
            _analyticsRepository = analyticsRepository;
            _panelRepository = panelRepository;
        }

        // GET panel/XXXX1111YYYY2222/analytics
        [HttpGet("{banelId}/[controller]")]
        public async Task<IActionResult> Get([FromRoute] string panelId)
        {
            var panelIdint = Convert.ToInt32(panelId);
            var panel = await _panelRepository.Query().FirstOrDefaultAsync(x => x.Id == panelIdint);
            //var panel = await _panelRepository.GetAsync(panelId);
            if (panel == null) return NotFound();

            var analytics = await _analyticsRepository.Query()
                .Where(x => x.PanelId == panelIdint).ToListAsync();

            var result = new OneHourElectricityListModel
            {
                OneHourElectricitys = analytics.Select(c => new OneHourElectricityModel
                {
                    Id = c.Id,
                    KiloWatt = c.KiloWatt,
                    DateTime = c.DateTime
                })
            };

            return Ok(result);
        }

        // GET panel/XXXX1111YYYY2222/analytics/day
        [HttpGet("{panelId}/[controller]/day")]
        public async Task<IActionResult> DayResults([FromRoute] string panelId)
        {
            var analytics = await GetList(panelId);
            var resultlist = Calculate(analytics);
            return Ok(resultlist);
        }

        private async Task<List<OneHourElectricity>> GetList(string panelId)
        {
            var panelIdint = Convert.ToInt32(panelId);
            var analytics = await _analyticsRepository.Query().Where(x => x.PanelId == panelIdint).ToListAsync();
            return analytics;
        }

        public List<OneDayElectricityModel> Calculate(List<OneHourElectricity> models)
        {
            var data = models.GroupBy(x => x.DateTime.ToShortDateString()).Select(x => new OneDayElectricityModel
            {
                Sum = x.Sum(p => p.KiloWatt),
                Average = x.Average(p => p.KiloWatt),
                Maximum = x.Max(p => p.KiloWatt),
                Minimum = x.Min(p => p.KiloWatt),
                DateTime = x.First().DateTime
            }).OrderByDescending(x => x.DateTime).ToList();
            return data;
        }

        // POST panel/XXXX1111YYYY2222/analytics
        [HttpPost("{panelId}/[controller]")]
        public async Task<IActionResult> Post([FromRoute] string panelId, [FromBody] OneHourElectricityModel value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var panelIdint = Convert.ToInt32(panelId);
            var oneHourElectricityContent = new OneHourElectricity
            {
                PanelId = panelIdint,
                KiloWatt = value.KiloWatt,
                DateTime = DateTime.UtcNow
            };

            await _analyticsRepository.InsertAsync(oneHourElectricityContent);

            var result = new OneHourElectricityModel
            {
                Id = oneHourElectricityContent.Id,
                KiloWatt = oneHourElectricityContent.KiloWatt,
                DateTime = oneHourElectricityContent.DateTime
            };

            return Created($"panel/{panelId}/analytics/{result.Id}", result);
        }
    }
}