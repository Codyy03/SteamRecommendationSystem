namespace steam_discovery_platform.Server.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
