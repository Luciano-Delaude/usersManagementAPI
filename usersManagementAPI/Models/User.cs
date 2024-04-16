using System.Text.Json.Serialization;

namespace usersManagementAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public DateTime? UserBirthdate { get; set; }

    public bool? IsActive { get; set; } = true;
}
