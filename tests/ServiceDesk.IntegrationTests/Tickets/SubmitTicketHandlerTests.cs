using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Data;
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
        }
    }
}
