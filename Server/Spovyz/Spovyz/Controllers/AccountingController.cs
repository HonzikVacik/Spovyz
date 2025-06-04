using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spovyz.IServices;
using Spovyz.Transport_models;
using System;

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly IAccountingService _accountingService;

        public AccountingController(IAccountingService accountingService)
        {
            _accountingService = accountingService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error, AccountingDataShort[]? accountingDataShorts) result = await _accountingService.Get(UserName);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.accountingDataShorts);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound("Uživatel nenalezen");

            (ValidityControl.ResultStatus status, string? error, AccountingDataLong[]? accountingDataLongs) result = await _accountingService.GetAll(UserName);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.accountingDataLongs);
        }
    }
}