namespace SA.Accounting.Core.Entities.Identity;

public class ApplicationRole : IdentityRole<int>
{
    public bool IsDeleted { get; set; }
}
