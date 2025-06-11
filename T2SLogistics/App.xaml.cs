using T2SLogistics.Helpers;
using T2SLogistics.Models;
using T2SLogistics.Services;
using T2SLogistics.ViewModels.Expedicao;
using T2SLogistics.ViewModels.Recepcao;
using T2SLogistics.Views;

namespace T2SLogistics
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {

            DependencyService.Register<MockDataStore>();

            Settings.TipoDocEncomendaFornec = "1";
            Settings.TipoDocEncomendaCliente = "2";
            Settings.TipoDocEncomendaEntidades = "3";


            Settings.UserName = "t2s";
            Settings.Password = "t2s";

            if (string.IsNullOrEmpty(Settings.UrlApiBase))
            {
                return new Window(new SettingsPage());

            }
            else
            {
                //TODO: remove this
                if (Settings.NifEmpresa == "500074496")
                {

                    try
                    {
                        LoadLotesCaiaca();
                        //LoadFornecedoresCaiaca();
                        CleanLocalFornecsArtigos();

                        var recVM = new RecepcaoCaiacaHomeViewModel();

                        var _fornecsInDb = Task.Run(async () => await recVM.GetFornecedoresFromApi()).Result;
                    }
                    catch (System.Exception ex)
                    {
                        Settings.UserName = "";
                        Settings.Password = "";
                    }

                }

                if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password) && !Settings.UserMustResetPassword)
                    return new Window(new AppShell());
                else

                    return new Window(new LoginPage());

            }
        }

        //public App()
        //{
        //    InitializeComponent();

        //    DependencyService.Register<MockDataStore>();

        //    Settings.TipoDocEncomendaFornec = "1";
        //    Settings.TipoDocEncomendaCliente = "2";
        //    Settings.TipoDocEncomendaEntidades = "3";


        //    Settings.UserName = "t2s";
        //    Settings.Password = "t2s";

        //    if (string.IsNullOrEmpty(Settings.UrlApiBase))
        //    {

        //        MainPage = new SettingsPage();

        //    }
        //    else
        //    {
        //        //TODO: remove this
        //        if (Settings.NifEmpresa == "500074496")
        //        {

        //            try
        //            {
        //                LoadLotesCaiaca();
        //                //LoadFornecedoresCaiaca();
        //                CleanLocalFornecsArtigos();

        //                var recVM = new RecepcaoCaiacaHomeViewModel();

        //                var _fornecsInDb = Task.Run(async () => await recVM.GetFornecedoresFromApi()).Result;
        //            }
        //            catch (System.Exception ex)
        //            {
        //                Settings.UserName = "";
        //                Settings.Password = "";
        //            }

        //        }

        //        if (!string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password) && !Settings.UserMustResetPassword)
        //            MainPage = new AppShell();
        //        else
        //            MainPage = new LoginPage();

        //    }



        //}

        private async void LoadLotesCaiaca()
        {
            var _expedicaoVM = new ExpedicaoCaiacaViewModel();
            await _expedicaoVM.GetLotesFromApi();
        }


        private async void LoadFornecedoresCaiaca()
        {
            var _recVM = new RecepcaoCaiacaHomeViewModel();
            await _recVM.GetFornecedoresFromApi();
        }

        private void CleanLocalFornecsArtigos()
        {
            var _fornecs = new FornecedoresCaiaca();
            _fornecs.UpdateLocalDb();

            var _artigos = new ArtigosCaiaca();
            _artigos.UpdateLocalArtigosRecepcao();

        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}