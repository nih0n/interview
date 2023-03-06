using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solution.API.Requests;
using Solution.Application.UseCases.AddCompanyCost;
using Solution.Application.UseCases.CloseCompany;
using Solution.Application.UseCases.CreateCompany;
using Solution.Application.UseCases.GetCompany;
using System.Threading.Tasks;

namespace Solution.API.Controllers
{
    [Route("empresa")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IGetCompanyUseCase _getCompanyUseCase;
        private readonly ICreateCompanyUseCase _createCompanyUseCase;
        private readonly ICloseCompanyUseCase _closeCompanyUseCase;
        private readonly IAddCompanyCostUseCase _addCompanyCostUseCase;

        public CompanyController(
            IGetCompanyUseCase getCompanyUseCase,
            ICreateCompanyUseCase createCompanyUseCase,
            ICloseCompanyUseCase closeCompanyUseCase,
            IAddCompanyCostUseCase addCompanyCostUseCase)
            => (_getCompanyUseCase, _createCompanyUseCase, _closeCompanyUseCase, _addCompanyCostUseCase)
            = (getCompanyUseCase, createCompanyUseCase, closeCompanyUseCase, addCompanyCostUseCase);

        [Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(string id)
        {
            try
            {
                var result = _getCompanyUseCase.Execute(new(id));

                if (result.Success)
                    return Ok(result.Data);

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateCompanyRequest request)
        {
            try
            {
                var result = await _createCompanyUseCase.Execute(new(request.Id, request.Status));

                if (result.Success)
                    return Ok();

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put(
            [FromRoute] string id,
            [FromBody] AddCompanyCostRequest request)
        {
            try
            {
                var result = await _addCompanyCostUseCase.Execute(new(id, new(request.Year, request.Type, request.Value)));

                if (result.Success)
                    return Ok();

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _closeCompanyUseCase.Execute(new(id));

                if (result.Success)
                    return Ok();

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
