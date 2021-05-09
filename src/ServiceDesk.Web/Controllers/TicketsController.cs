using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Domain.Tickets;

namespace ServiceDesk.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(
            SubmitTicketCommand command, 
            CancellationToken cancellationToken)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }            
            
            await _mediator.Send(command, cancellationToken);
            
            return View();
        }

    }
}