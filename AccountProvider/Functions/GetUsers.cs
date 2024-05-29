using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountProvider.Functions
{
    public class GetUsers
    {
        private readonly ILogger<GetUsers> _logger;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public GetUsers(ILogger<GetUsers> logger, IDbContextFactory<DataContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [Function("GetUsers")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var users = await context.Users.ToListAsync();

                return new OkObjectResult(users);
            }
            catch(Exception ex)
            {
                return new NotFoundObjectResult("user not found" + ex.Message);
            }
           
        }
    }
}
