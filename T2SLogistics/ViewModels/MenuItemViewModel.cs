using CommunityToolkit.Mvvm.Input;

namespace T2SLogistics.ViewModels;

/// <summary>Uma entrada do menu principal: ícone (glifo Tabler), título e ação ao tocar.</summary>
public sealed class MenuItemViewModel
{
    public string Title { get; }
    public string Glyph { get; }
    public bool IsAccent { get; }
    public IAsyncRelayCommand TapCommand { get; }

    /// <summary>
    /// Ícone como <see cref="FontImageSource"/> construído em código. Renderizar a icon-font via
    /// um <c>Image</c> (em vez de <c>Label</c> com binding) evita o problema de medição que deixava
    /// os glifos em branco quando o texto vinha de um binding.
    /// </summary>
    public ImageSource Icon { get; }

    public MenuItemViewModel(string title, string glyph, Func<Task> onTap, bool isAccent = false)
    {
        Title = title;
        Glyph = glyph;
        IsAccent = isAccent;
        TapCommand = new AsyncRelayCommand(onTap);

        Icon = new FontImageSource
        {
            FontFamily = "TablerIcons",
            Glyph = glyph,
            Size = 18,
            Color = Res.Color("BrandRedDark")
        };
    }
}
