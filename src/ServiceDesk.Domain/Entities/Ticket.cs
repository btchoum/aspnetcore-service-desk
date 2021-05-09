using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        
        [StringLength(200)]
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Details { get; set; }
        
        [Required]
        [StringLength(200)]
        public string SubmitterEmail { get; set; }

        [Required]
        [StringLength(200)]
        public string SubmitterName { get; set; }
        
        [Required]
        public DateTime DateSubmitted { get; set; }
        public DateTime? DateClosed { get; set; }
        public TicketStatus Status { get; set; }
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
