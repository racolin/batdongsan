using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
public class Role : IdentityRole<int>
{
    public Role() : base() {}

    public Role(string role) : base(role) {}
    
    public virtual ICollection<UserRole> UserRoles { get; set; }

    public string VisibleModule {  get; set; } = string.Empty;
}