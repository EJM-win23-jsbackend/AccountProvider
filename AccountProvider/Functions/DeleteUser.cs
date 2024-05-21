using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountProvider.Functions
{
    public class DeleteUser
    {
        private readonly ILogger<DeleteUser> _logger;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public DeleteUser(ILogger<DeleteUser> logger, IDbContextFactory<DataContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [Function("Function")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleteuser/{userId}")] HttpRequest req, string userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(userId);

            if(user == null)
            {
                return new NotFoundObjectResult($"User with id: {userId} was not found");
            }

            context.User.Remove(user);
            await context.SaveChangesAsync();

            return new OkObjectResult($"User with id: {userId} have been deleted");
        }
    }
}
