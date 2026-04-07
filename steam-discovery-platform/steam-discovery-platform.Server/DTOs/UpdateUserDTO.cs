namespace steam_discovery_platform.Server.DTOs
{
    public class UpdateUserDTO
    {
        public string? UserName {  get; set; }
        public Guid UserId {  get; set; }
        public string? Email {  get; set; }
    }
}
