using FestivalDataApi.DataModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FestivalDataApi.Controllers
{
    /// <summary>
    /// Controller for Festival data management
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    [ApiController]
    [RequireHttps]
    public class FestivalDataController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FestivalDataController> _logger;

        /// <summary>
        /// Constructor for FestivalDataController
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public FestivalDataController(IMediator mediator,
                         ILogger<FestivalDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets all festival data and return them in the specified format
        /// </summary>
        [AllowAnonymous]
        [HttpGet("api/v1/recordlabels")]
        [ProducesResponseType(typeof(RecordLabelsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordLabelsAsync()
        {
            var request = new RecordLabelsRequest
            {
                // Note: This request could be used to create and assign pagination 
                // properties such as defined in this Stripe documentation https://stripe.com/docs/api/pagination 
            };

            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
