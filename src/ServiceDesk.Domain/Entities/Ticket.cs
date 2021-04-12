using System;

namespace ServiceDesk.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateClosed { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }

    public enum TicketStatus
    {
        New = 1,
        Active,
        Pending,
        Resolved,
        Closed,
        Cancelled
    }
}
