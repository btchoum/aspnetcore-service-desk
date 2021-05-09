using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceDesk.Domain.Tickets;
using Xunit;

namespace ServiceDesk.IntegrationTests.Tickets
{
    public class CustomerUpdateTests : IntegrationTestBase
    {
        [Fact]
        public async Task Customer_update_adds_comment()
        {
            var db = MakeDbContext(nameof(Customer_update_adds_comment));
            var existingTicketId = await GivenExistingTicket(db);
            
            var updateCommand = new TicketCustomerUpdateCommand
            {
                Id = existingTicketId,
                Comment = $"Some New Comment {Guid.NewGuid()}",
                SubmitterId = $"test{Guid.NewGuid()}@example.com"
            };
            var handler = new TicketUpdateHandler(db);
            await handler.Handle(updateCommand, CancellationToken.None);

            var detailHandler = new TicketDetailsHandler(db);
            var detailsRequest = new TicketDetailsRequest
            {
                Id = existingTicketId
            };
            var ticket = await detailHandler.Handle(detailsRequest, CancellationToken.None);

            ticket.Comments.Count.Should().Be(1);
            ticket.Comments[0].Text.Should().Be(updateCommand.Comment);
            ticket.Comments[0].SubmitterId.Should().Be(updateCommand.SubmitterId);
            ticket.Comments[0].DateEntered.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
