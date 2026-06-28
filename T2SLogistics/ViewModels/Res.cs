namespace T2SLogistics.ViewModels;

/// <summary>Acesso tipado a cores definidas em Resources/Styles/Colors.xaml.</summary>
internal static class Res
{
    public static Color Color(string key) =>
        Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color c
            ? c
            : Colors.Transparent;
}
