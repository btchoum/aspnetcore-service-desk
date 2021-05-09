using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Data;
using ServiceDesk.Domain.Entities;
using ServiceDesk.Domain.Tickets;
using Xunit;

namespace ServiceDesk.IntegrationTests.Tickets
{
    public class SubmitTicketHandlerTests
    {
        [Fact]
        public async Task Create_is_successful()
        {
            var command = new SubmitTicketCommand
            {
                Title = $"Title - {Guid.NewGuid()}",
                Details = $"Details - {Guid.NewGuid()}",
                SubmitterEmail = $"email{Guid.NewGuid()}@example.com",
                SubmitterName = $"Test User {Guid.NewGuid()}"
            };

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nameof(Create_is_successful))
                .Options;
            
            var handler = new SubmitTicketHandler(new ApplicationDbContext(contextOptions));

            var result = await handler.Handle(command, CancellationToken.None);

            result.TicketId.Should().NotBeEmpty();

            var db = new ApplicationDbContext(contextOptions);
            var ticket = await db.Tickets.FirstOrDefaultAsync();
            ticket.Should().NotBeNull();
            ticket.Should().BeEquivalentTo(command);
            ticket.Status.Should().Be(TicketStatus.New);
            ticket.DateSubmitted.Should().BeCloseTo(DateTime.UtcNow, 2000);
            ticket.DateClosed.Should().BeNull();
        }
    }
}
