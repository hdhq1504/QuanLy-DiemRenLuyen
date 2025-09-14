public static class SessionManager
{
    public static int UserId { get; set; }
    public static string Username { get; set; }
    public static string RoleCode { get; set; }
    public static bool IsLoggedIn { get; set; } = false;

    public static void Logout()
    {
        UserId = 0;
        Username = string.Empty;
        RoleCode = string.Empty;
        IsLoggedIn = false;
    }
}