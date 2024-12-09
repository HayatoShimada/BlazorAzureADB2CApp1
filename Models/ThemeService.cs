using MudBlazor;

public class ThemeService
{
    public event Action? OnThemeChanged; // イベントを追加

    private bool _isDarkMode = true;
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnThemeChanged?.Invoke(); // 状態変更時にイベント発行
            }
        }
    }

    public string DarkLightModeButtonIcon => IsDarkMode switch
    {
        true => Icons.Material.Rounded.AutoMode,
        false => Icons.Material.Outlined.DarkMode,
    };
}
