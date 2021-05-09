using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Text { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        [StringLength(200)]
        public string SubmitterId { get; set; }

        [Required]
        public DateTime DateEntered { get; set; }
    }
}