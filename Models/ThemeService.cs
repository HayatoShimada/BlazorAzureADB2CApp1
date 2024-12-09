using MudBlazor;

public class ThemeService
{
    public event Action? OnThemeChanged; // �C�x���g��ǉ�

    private bool _isDarkMode = true;
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnThemeChanged?.Invoke(); // ��ԕύX���ɃC�x���g���s
            }
        }
    }

    public string DarkLightModeButtonIcon => IsDarkMode switch
    {
        true => Icons.Material.Rounded.AutoMode,
        false => Icons.Material.Outlined.DarkMode,
    };
}
