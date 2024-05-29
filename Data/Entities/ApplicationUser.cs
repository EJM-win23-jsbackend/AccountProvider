

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfileImage { get; set; }
    public string? Biography { get; set; }
    public string? UserAddressId { get; set; }

    [JsonIgnore]
    public virtual UserAddress? UserAddress { get; set; }
}

