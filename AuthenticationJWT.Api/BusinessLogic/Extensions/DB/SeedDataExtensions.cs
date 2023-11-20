namespace AuthenticationJWT.Api.BusinessLogic.Extensions.DB;

public static class SeedDataExtensions
{
    public static void SeedUsers(this ModelBuilder builder)
    {
        var hasher = new PasswordHasher<User>();
        builder
            .Entity<User>()
            .HasData(
                new User
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb7",
                    UserName = "Aamir Khan",
                    Email = "AamirKhan@Example.com",
                    PasswordHash = hasher.HashPassword(null, "Allah"),
                    LockoutEnabled = true,
                    EmailConfirmed = true,
                    Password = "Allah",
                }
            );
    }
}
