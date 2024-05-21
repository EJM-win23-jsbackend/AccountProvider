using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace AccountProvider.Functions
{
    public class UpdateUser
    {
        private readonly ILogger<UpdateUser> _logger;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public UpdateUser(ILogger<UpdateUser> logger, IDbContextFactory<DataContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }

        [Function("UpdateUser")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "user/{userId}")] HttpRequest req, string userId)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserAccountModel>(requestBody);

            if (data == null)
            {
                return new BadRequestObjectResult("Invalid");
            }

            _logger.LogInformation($"request for user with id: {userId}");
            _logger.LogInformation($"recieved data from user: {requestBody}");

            using var context = _dbContextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return new NotFoundResult();
            }
            else
            {
                user.UserName = data.UserName;
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                user.Email = data.Email;
                user.PhoneNumber = data.PhoneNumber;
                
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return new OkObjectResult("Your profile information was updated!");
            } 
        }
    }
}
