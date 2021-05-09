using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceDesk.Domain.Entities;
using ServiceDesk.Domain.Tickets;
using Xunit;

namespace ServiceDesk.IntegrationTests.Tickets
{
    public class SubmitTicketTests : IntegrationTestBase
    {
        [Fact]
        public async Task Create_Details_RoundTrip()
        {
            var command = new SubmitTicketCommand
            {
                Title = $"Title - {Guid.NewGuid()}",
                Details = $"Details - {Guid.NewGuid()}",
                SubmitterEmail = $"email{Guid.NewGuid()}@example.com",
                SubmitterName = $"Test User {Guid.NewGuid()}"
            };

            var dbContext = MakeDbContext(nameof(Create_Details_RoundTrip));
            
            var handler = new SubmitTicketHandler(dbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            result.TicketId.Should().NotBeEmpty();

            var detailHandler = new TicketDetailsHandler(dbContext);
            var detailsRequest = new TicketDetailsRequest
            {
                Id = result.TicketId
            };
            var ticket = await detailHandler.Handle(detailsRequest, CancellationToken.None);
            
            ticket.Should().NotBeNull();
            ticket.Should().BeEquivalentTo(command);
            ticket.Status.Should().Be(TicketStatus.New);
            ticket.DateSubmitted.Should().BeCloseTo(DateTime.UtcNow, 2000);
            ticket.DateClosed.Should().BeNull();
        }

    }
}