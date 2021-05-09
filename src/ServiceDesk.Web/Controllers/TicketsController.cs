using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Domain.Tickets;
using ServiceDesk.Web.Models;

namespace ServiceDesk.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("submit")]
        public IActionResult Submit()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("submit")]
        public async Task<IActionResult> Submit(
            SubmitTicketCommand command,
            CancellationToken cancellationToken)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Details), new { id = result.TicketId });
        }

        [HttpGet("tickets/{id:guid}")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var request = new TicketDetailsRequest
            {
                Id = id
            };

            var ticket = await _mediator.Send(request, cancellationToken);

            if (ticket == null)
            {
                return NotFound();
            }

            var viewModel = new TicketDetailsViewModel(ticket);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("update")]
        public async Task<IActionResult> Update(
            Guid id,
            TicketCustomerUpdateCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                command.Id = id;
            }

            command.SubmitterId = User.Identity?.Name;
            
            await _mediator.Send(command, cancellationToken);

            return RedirectToAction(nameof(Details), new { id = command.Id });
        }
    }
}