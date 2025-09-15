public static class SessionManager
{
    public static string UserId { get; set; }
    public static string Username { get; set; }
    public static string RoleCode { get; set; }
    public static bool IsLoggedIn { get; set; } = false;

    public static void Logout()
    {
        UserId = string.Empty;
        Username = string.Empty;
        RoleCode = string.Empty;
        IsLoggedIn = false;
    }
}