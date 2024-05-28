using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "user/{userId}")] HttpRequestData req, string userId)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserAccountModel>(requestBody);
            _logger.LogInformation($"recieved data from user: {requestBody}");


            if (data == null)
            {
                var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("invalid");
                return badRequestResponse;
            }

            _logger.LogInformation($"request for user with id: {userId}");
            
            using var context = _dbContextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(userId);

            if(user == null)
            {
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("user was not found");
                return notFoundResponse;
            }

            if(data.FirstName != null) user.FirstName = data.FirstName;
            if(data.LastName != null) user.LastName = data.LastName;
            if(data.Email != null) user.Email = data.Email;
            if(data.PhoneNumber != null) user.PhoneNumber = data.PhoneNumber;
            if(data.UserName != null) user.UserName = data.UserName;


            await context.SaveChangesAsync();
                
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);
            return response;
            
        }
    }
}
