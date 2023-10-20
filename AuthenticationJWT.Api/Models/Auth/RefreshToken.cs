namespace AuthentificationJWT.Api.Models.Auth;

public class RefreshToken
{
    [Key]
    public string Token { get; set; } = default!;
    public string JwtId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Used { get; set; }
    public bool Invalidated { get; set; }
    public string UserId { get; set; } = default!;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!;
}
