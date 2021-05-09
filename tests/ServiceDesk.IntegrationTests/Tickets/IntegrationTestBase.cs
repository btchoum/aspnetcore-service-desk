using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Data;
using ServiceDesk.Domain.Tickets;

namespace ServiceDesk.IntegrationTests.Tickets
{
    public class IntegrationTestBase
    {
        protected static async Task<Guid> GivenExistingTicket(ApplicationDbContext db)
        {
            var command = new SubmitTicketCommand
            {
                Title = $"Title - {Guid.NewGuid()}",
                Details = $"Details - {Guid.NewGuid()}",
                SubmitterEmail = $"email{Guid.NewGuid()}@example.com",
                SubmitterName = $"Test User {Guid.NewGuid()}"
            };

            var handler = new SubmitTicketHandler(db);

            var result = await handler.Handle(command, CancellationToken.None);

            return result.TicketId;
        }

        protected static ApplicationDbContext MakeDbContext(string databaseName)
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            var dbContext = new ApplicationDbContext(contextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}