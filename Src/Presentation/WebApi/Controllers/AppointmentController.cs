using Application.Appointments.Commands.CreateAppointment;
using Application.Appointments.Commands.CreateEarliestAppointment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using Shared.Model;

namespace WebApi.Controllers
{
    [Route("appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpPost("set-appointment")]
        public async Task<IActionResult> SetAppointment(CreateAppointmentRequest request)
        {
            ApiResponse<CreateAppointmentResultDto> actionResult = new();

            try
            {
                CreateAppointmentResultDto result = await _mediator.Send(request);
                actionResult.Data = result;
                return Ok(actionResult);

            }
            catch (ApiException ex)
            {
                actionResult.Errors.Add(new Error
                {
                    Message = ex.Message,
                    Code = ex.Code
                });
                return BadRequest(actionResult);
            }

        }

        [HttpPost("set-earliest-appointment")]
        public async Task<IActionResult> SetEarliestAppointment(CreateEarliestAppointmentRequest request)
        {
            ApiResponse<CreateEarliestAppointmentResultDto> actionResult = new();

            try
            {
                CreateEarliestAppointmentResultDto result = await _mediator.Send(request);
                actionResult.Data = result;
                return Ok(actionResult);

            }
            catch (ApiException ex)
            {
                actionResult.Errors.Add(new Error
                {
                    Message = ex.Message,
                    Code = ex.Code
                });
                return BadRequest(actionResult);
            }
        }
    }
}
