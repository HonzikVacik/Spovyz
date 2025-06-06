using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spovyz.IServices;
using Spovyz.Transport_models;
using static Spovyz.ValidityControl;

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

        [HttpGet("FinanceResult")]
        [Authorize(Roles = "Accountant,Owner")]
        public async Task<IActionResult> GetFinanceResult(bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error, FinanceResult financeResult) result = await _financeService.GetFinanceResult(UserName, Current_planned);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.financeResult);
        }

        [HttpGet]
        [Authorize(Roles = "Accountant,Owner")]
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

        [HttpGet("{id}")]
        [Authorize(Roles = "Accountant,Owner")]
        public async Task<IActionResult> Get(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, FinanceData? finance, string? error) result = await _financeService.GetFinanceById(UserName, id);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.finance);
        }

        [HttpPost]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> Post(string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) result = await _financeService.AddFinance(UserName, Name, Cost, Description, Income_expenditure, Current_planned);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> Put(uint id, string Name, uint Cost, string? Description, bool Income_expenditure, bool Current_planned)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) result = await _financeService.UpdateFinance(UserName, id, Name, Cost, Description, Income_expenditure, Current_planned);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> Delete(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error) result = await _financeService.DeleteFinance(UserName, id);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }
    }
}
