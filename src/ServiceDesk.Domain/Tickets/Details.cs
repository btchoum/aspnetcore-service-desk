using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Data;
using ServiceDesk.Domain.Entities;

namespace ServiceDesk.Domain.Tickets
{
    public class TicketDetailsRequest : IRequest<Ticket>
    {
        public Guid Id { get; set; }
    }

    public class TicketDetailsHandler
        : IRequestHandler<TicketDetailsRequest, Ticket>
    {
        private readonly ApplicationDbContext _db;

        public TicketDetailsHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Ticket> Handle(
            TicketDetailsRequest request, 
            CancellationToken cancellationToken)
        {
            var ticket = await _db.Tickets
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            return ticket;
        }
    }
}
