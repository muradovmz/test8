using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using {{cookiecutter.ProjectName}}.Api.Attribute;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Command;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Query;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;

namespace {{cookiecutter.ProjectName}}.Api.Controller
{
    /// <summary>
    /// Mediator based controllers. We can make our controllers very thin and delegate required logics to handlers
    /// </summary>
    public class BankController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet("{id:int}")]
        [ApiOperation("Get Bank Details")]
        [ApiSuccessResponse(200, "Bank information for the provided bank id", typeof(BankResponse))]
        [ApiErrorResponse(404, "Bank id doesn't exist")]
        public async Task<IActionResult> Bank([FromRoute, Required] int id) => Ok(await _mediator.Send(new BankByIdQuery { BankId = id }));


        [HttpPost]
        [ApiOperation("Creates a new bank")]
        [ApiSuccessResponse(201, "The Id of the created bank", typeof(BankCreatedResponse))]
        [ApiErrorResponse(400, "Input request has invalid values")]
        public async Task<IActionResult> CreateBank([FromBody] CreateBankCommand bank) => CreatedResult(await _mediator.Send(bank));
    }
}