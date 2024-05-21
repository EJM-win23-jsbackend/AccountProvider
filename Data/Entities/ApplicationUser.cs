

using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfileImage { get; set; }
    public string? Biography { get; set; }
    public string? UserAddressId { get; set; }
    public virtual UserAddress? UserAddress { get; set; }
}

