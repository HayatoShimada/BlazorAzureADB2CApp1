public class UserStateService
{
    public static string Email { get; set; } = string.Empty;
    public static string Username { get; set; } = string.Empty;

    public static bool IsLoggedIn => !string.IsNullOrEmpty(Username);
}