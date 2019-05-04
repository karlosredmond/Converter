using Converter.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Converter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private IUtility _utility;

        public ConverterController(IMapPeriod periodMapper, IUtility utility)
        {
            _utility = utility;
        }

        // GET api/converter/[FT] 90:00.000
        [HttpGet("{matchTimeStr}")]
        public string Get(string matchTimeStr = "")
        {
            if (!_utility.CheckMatchTimeInput(matchTimeStr))
            {
                return "INVALID";
            }

            _utility.SetCorrectTime(matchTimeStr);

            return _utility.DisplayTime(); 
        }

        [HttpGet]
        public string Get()
        {
            return "Converter API";
        }
    }
}
