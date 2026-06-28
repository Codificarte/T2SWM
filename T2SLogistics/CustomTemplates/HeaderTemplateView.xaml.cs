namespace T2SLogistics.CustomTemplates;

public partial class HeaderTemplateView : ContentView
{
	private IDispatcherTimer _clockTimer;

	public HeaderTemplateView()
	{
		InitializeComponent();
        day.Text = DateTime.Now.DayOfWeek.ToString();
        date.Text = DateTime.Now.ToString("MMMM dd, yyyy");

        _clockTimer = Dispatcher.CreateTimer();
        _clockTimer.Interval = TimeSpan.FromSeconds(1);
        _clockTimer.Tick += (s, e) =>
        {
            clockTimerLabel.Text = DateTime.Now.ToString("hh:mm:ss");
        };

        // Só corre enquanto a view está no ecrã. Parar ao sair evita timers acumulados (um por
        // cada página navegada) a sobrecarregar a UI thread — causa provável do ANR em Debug.
        Loaded += (s, e) => _clockTimer.Start();
        Unloaded += (s, e) => _clockTimer.Stop();
    }
    
    public static readonly BindableProperty IsOperatorViewVisibleProperty =
    BindableProperty.Create(nameof(IsOperatorViewVisible), typeof(bool), typeof(HeaderTemplateView), default(bool));

    public bool IsOperatorViewVisible
    {
        get => (bool)GetValue(IsOperatorViewVisibleProperty);
        set => SetValue(IsOperatorViewVisibleProperty, value);
    }
    public static readonly BindableProperty IsTimerViewViewVisibleProperty =
    BindableProperty.Create(nameof(IsTimerViewViewVisible), typeof(bool), typeof(HeaderTemplateView), default(bool));

    public bool IsTimerViewViewVisible
    {
        get => (bool)GetValue(IsTimerViewViewVisibleProperty);
        set => SetValue(IsTimerViewViewVisibleProperty, value);
    }
}