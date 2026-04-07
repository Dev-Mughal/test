namespace Common.Models
{
    //[ExcludeFromApiReference]
    public class AuthorizedUserDto
    {
        public required long Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required int BusinessId { get; set; }
        public string TimeZone { get; set; } = "UTC";
    }
}

