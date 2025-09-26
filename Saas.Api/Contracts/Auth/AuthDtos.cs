namespace Saas.Api.Contracts.Auth
{
    public sealed class RegisterRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }

    public sealed class LoginRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }

    public sealed class AuthResponse
    {
        public string Token { get; init; } = default!;
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
    }

}
