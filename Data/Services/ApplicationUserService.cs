

using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public class ApplicationUserService
{

    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public ApplicationUserService(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public async Task<IActionResult> GetOneUserAsync(string id)
    {
        try
        {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var entityUserToGet = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                    if (entityUserToGet != null)
                    {
                        return new OkObjectResult(entityUserToGet);
                    }
                }

            return new BadRequestObjectResult
                ( new 
                    { StatusCode = 400} 
                );
        }
        catch (Exception ex) 
        {
            return new BadRequestObjectResult(ex);
        }
    }

    public async Task<IActionResult> ExistingUserAsync(string entity)
    {
        try
        {
            if (entity != null)
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var entityExists = await context.Users.FirstOrDefaultAsync(x => x.Id == entity);

                    if (entityExists != null)
                    {
                        return new OkObjectResult(entityExists);
                    }
                }
            }
            return new NotFoundObjectResult
                (new
                { StatusCode = 404 }
                );
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }

    public async Task<IActionResult> DeleteUserAsync(ApplicationUser entity)
    {
        try
        {
            if (entity != null)
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.User.Remove(entity);
                    await context.SaveChangesAsync();

                    return new OkResult();
                }
               
            }

            return new NotFoundObjectResult
                (new
                { StatusCode = 404 }
                );
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}
