namespace Identity.Library.Defaults
{
    public static class IdentityDefaults
    {
        public static string Authority { get; set; } = "https://localhost:5000";
        public static string AccountManagement { get; set; } = "https://localhost:5001";
        public static string AccountApi { get; set; } = "https://localhost:5002";

        public const string AuthenticationScheme = "oidc";

        public const string RoleClaimType = "role";
    }
}
