using MudBlazor;
public class ThemeService
{
    public bool IsDarkMode { get; set; } = true;

    public string DarkLightModeButtonIcon => IsDarkMode switch
    {
        true => Icons.Material.Rounded.AutoMode,
        false => Icons.Material.Outlined.DarkMode,
    };
}
