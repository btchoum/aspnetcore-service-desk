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
            TicketCustomerUpdateCommand request, 
            CancellationToken cancellationToken)
        {

            var ticket = await _db.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            
            var comment = new Comment
            {
                Text = request.Comment,
                DateEntered = DateTime.Now,
                TicketId = ticket.Id,
                //TODO: add authenticated user here ...
                SubmitterEmail = "test@example.com"
            };

            _db.Add(comment);

            await _db.SaveChangesAsync(cancellationToken);

            return new TicketUpdateResult();
        }
    }
}
