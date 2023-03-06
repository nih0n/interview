using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solution.API.Requests;
using Solution.Application.UseCases.AddCompanyToGroup;
using Solution.Application.UseCases.CreateGroup;
using Solution.Application.UseCases.GetGroup;
using Solution.Application.UseCases.GetGroupCompanies;
using Solution.Application.UseCases.GetGroupCosts;
using Solution.Domain;
using System;
using System.Threading.Tasks;

namespace Solution.API.Controllers
{
    [Route("grupo")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGetGroupUseCase _getGroupUseCase;
        private readonly IGetGroupCompaniesUseCase _getGroupCompaniesUseCase;
        private readonly IGetGroupCostsUseCase _getGroupCostsUseCase;
        private readonly ICreateGroupUseCase _createGroupUseCase;
        private readonly IAddCompanyToGroupUseCase _addCompanyToGroupUseCase;

        public GroupController(
            IGetGroupUseCase getGroupUseCase,
            IGetGroupCompaniesUseCase getGroupCompaniesUseCase,
            IGetGroupCostsUseCase getGroupCostsUseCase,
            ICreateGroupUseCase createGroupUseCase,
            IAddCompanyToGroupUseCase addCompanyToGroupUseCase)
            => (_getGroupUseCase, _getGroupCompaniesUseCase, _getGroupCostsUseCase, _createGroupUseCase, _addCompanyToGroupUseCase)
            = (getGroupUseCase, getGroupCompaniesUseCase, getGroupCostsUseCase, createGroupUseCase, addCompanyToGroupUseCase);

        [Route("{id:int}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(uint id)
        {
            try
            {
                var result = _getGroupUseCase.Execute(new(id));

                if (result.Success)
                    return Ok(result.Data);

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] DateTime date)
        {
            try
            {
                var result = _getGroupCompaniesUseCase.Execute(new(date));

                if (result.Success)
                    return Ok(result.Data);

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("custos/{id:int}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCompaniesCost(uint id)
        {
            try
            {
                var result = _getGroupCostsUseCase.Execute(new(id));

                if (result.Success)
                    return Ok(result.Data);

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id:int}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateGroupRequest request)
        {
            try
            {
                var result = await _createGroupUseCase.Execute(new(request.Id, request.Name, request.category));

                if (result.Success)
                    return Ok(result);

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id:int}")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put(
            [FromRoute] uint groupId,
            [FromQuery(Name = "Id_empresa")] CompanyId companyId)
        {
            try
            {
                var result = await _addCompanyToGroupUseCase.Execute(new(groupId, companyId));

                if (result.Success)
                    return Ok(result);

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}