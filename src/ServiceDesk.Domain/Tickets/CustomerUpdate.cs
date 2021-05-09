using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Data;
using ServiceDesk.Domain.Entities;

namespace ServiceDesk.Domain.Tickets
{
    public class TicketCustomerUpdateCommand : IRequest<TicketUpdateResult>
    {
        public string Comment { get; set; }
        public Guid Id { get; set; }
        public string SubmitterId { get; set; }
    }

    public class TicketUpdateResult
    {

    }

    public class TicketUpdateHandler : IRequestHandler<TicketCustomerUpdateCommand, TicketUpdateResult>
    {
        private readonly ApplicationDbContext _db;

        public TicketUpdateHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<TicketUpdateResult> Handle(
            TicketCustomerUpdateCommand command, 
            CancellationToken cancellationToken)
        {

            var ticket = await _db.Tickets
                .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);
            
            var comment = new Comment
            {
                Text = command.Comment,
                DateEntered = DateTime.UtcNow,
                TicketId = ticket.Id,
                SubmitterId = command.SubmitterId
            };

            _db.Add(comment);

            await _db.SaveChangesAsync(cancellationToken);

            return new TicketUpdateResult();
        }
    }
}
