namespace Common.Features.Auth.SignUp.DTOs
{
    public record CreateBusinessUserDto
    (
        string FirstName,
        string LastName,
        string Email,
        string Password
    );
}
