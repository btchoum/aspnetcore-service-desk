using System;
using ServiceDesk.Domain.Entities;

namespace ServiceDesk.Web.Models
{
    public class TicketDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
        public string DateSubmitted { get; set; }
        public string DateClosed { get; set; }
        public string TicketStatus { get; set; }

        public TicketDetailsViewModel(Ticket ticket)
        {
            Id = ticket.Id;
            Title = ticket.Title;
            Details = ticket.Details;
            SubmitterName = ticket.SubmitterName;
            SubmitterEmail = ticket.SubmitterEmail;
            DateSubmitted = ticket.DateSubmitted.ToShortDateString();
            if (ticket.DateClosed.HasValue)
            {
                DateClosed = ticket.DateClosed.Value.ToShortDateString();
            }

            TicketStatus = ticket.Status.ToString();
        }
    }
}