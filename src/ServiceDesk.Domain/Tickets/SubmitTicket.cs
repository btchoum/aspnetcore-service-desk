using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceDesk.Domain.Data;
using ServiceDesk.Domain.Entities;

namespace ServiceDesk.Domain.Tickets
{
    public class SubmitTicketCommand: IRequest<SubmitTicketResult>
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
    }

    public class SubmitTicketResult
    {
        public Guid TicketId { get; set; }
    }

    public class SubmitTicketHandler 
        : IRequestHandler<SubmitTicketCommand, SubmitTicketResult>
    {
        private readonly ApplicationDbContext _db;

        public SubmitTicketHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<SubmitTicketResult> Handle(
            SubmitTicketCommand request, 
            CancellationToken cancellationToken)
        {
            var ticket = new Ticket
            {
                DateSubmitted = DateTime.UtcNow,
                Title = request.Title,
                Details = request.Details,
                SubmitterEmail = request.SubmitterEmail,
                SubmitterName = request.SubmitterName,
                TicketStatus = TicketStatus.New
            };

            _db.Add(ticket);

            await _db.SaveChangesAsync(cancellationToken);

            var result = new SubmitTicketResult
            {
                TicketId = ticket.Id
            };

            return result;
        }
    }
}
