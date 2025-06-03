using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spovyz.IServices;
using Spovyz.Transport_models;
using static Spovyz.ValidityControl;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        // GET: api/<FinanceController>
        [HttpGet("FinanceResult")]
        [Authorize]
        public async Task<IActionResult> GetFinanceResult(bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error, FinanceResult financeResult) result = await _financeService.GetFinanceResult(UserName, Current_planned);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.financeResult);
        }

        // GET: api/<FinanceController>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(bool Income_expenditure, bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();
            
            (ValidityControl.ResultStatus status, List<NameBasic>? finances, string? error) result = await _financeService.GetAllFinances(UserName, Income_expenditure, Current_planned);

            if(result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.finances);
        }

        // GET api/<FinanceController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, FinanceData? finance, string? error) result = await _financeService.GetFinanceById(UserName, id);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.finance);
        }

        // POST api/<FinanceController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error) result = await _financeService.AddFinance(UserName, Name, Cost, Description, Income_expenditure, Current_planned);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }

        // PUT api/<FinanceController>/5
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(uint id, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error) result = await _financeService.UpdateFinance(UserName, id, Name, Cost, Description, Income_expenditure, Current_planned);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }

        // DELETE api/<FinanceController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error) result = await _financeService.DeleteFinance(UserName, id);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }
    }
}
