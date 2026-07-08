using T2SLogistics.Views;

namespace T2SLogistics
{
    public partial class AppShell : Shell
    {
        public AppShell(MainMenuPage mainMenu)
        {
            InitializeComponent();

            // Conteúdo raiz: menu principal (resolvido por DI, com o seu ViewModel).
            Items.Add(new ShellContent
            {
                Title = "Menu",
                Route = "menu",
                Content = mainMenu
            });

            // Rotas reutilizáveis (lista e detalhe). Resolvidas por DI ao navegar com GoToAsync.
            Routing.RegisterRoute(Routes.Movements, typeof(MovementListPage));
            Routing.RegisterRoute(Routes.MovementDetail, typeof(MovementDetailPage));
            Routing.RegisterRoute(Routes.ReceptionReading, typeof(ReceptionReadingPage));
            Routing.RegisterRoute(Routes.ManagePin, typeof(ManagePinPage));
        }
    }
}
