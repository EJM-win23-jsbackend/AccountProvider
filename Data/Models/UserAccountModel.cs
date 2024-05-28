
using Data.Entities;

namespace Data.Models;

public class UserAccountModel : ApplicationUser
{
    public string? NewPassword { get; set; }
}
