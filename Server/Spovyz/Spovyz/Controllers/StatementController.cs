using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spovyz.IServices;
using Spovyz.Models;
using Spovyz.Services;
using Spovyz.Transport_models;
using System;
using static Spovyz.Models.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spovyz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementController : ControllerBase
    {
        private readonly IStatementService _statementService;

        public StatementController(IStatementService statementService)
        {
            _statementService = statementService;
        }

        // GET: api/<Statement>
        [HttpGet("StatementDataShort")]
        [Authorize]
        public async Task<IActionResult> Get(byte Day, uint AccountingId)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error, StatementDataShort[]? statementDataShorts) result = await _statementService.GetDay(UserName, Day, AccountingId);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.statementDataShorts);
        }

        // GET api/<Statement>/5
        [HttpGet("StatementDataLong")]
        [Authorize]
        public async Task<IActionResult> Get(uint AccountingId)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error, StatementDataLong? statementDataLong) result = await _statementService.GetMonth(UserName, AccountingId);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok(result.statementDataLong);
        }

        // POST api/<Statement>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(byte StatementType, DateOnly Datum, byte PocetHodin, string? Description)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error) result = await _statementService.AddStatement(UserName, StatementType, Datum, PocetHodin, Description);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }

        // DELETE api/<Statement>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(uint id)
        {
            string? UserName = User.Identity?.Name?.ToString();
            if (UserName == null)
                return NotFound();

            (ValidityControl.ResultStatus status, string? error) result = await _statementService.DeleteStatement(UserName, id);

            if (result.status == ValidityControl.ResultStatus.Error)
                return BadRequest(result.error);
            if (result.status == ValidityControl.ResultStatus.NotFound)
                return NotFound(result.error);
            return Ok();
        }
    }
}
