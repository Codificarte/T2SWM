using T2SLogistics.Resx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        private LocalizationResourceManager()
        {
            AppResources.Culture = CultureInfo.CurrentCulture;
        }

        public static LocalizationResourceManager Instance { get; } = new();

        public string this[string resourceKey]
            => AppResources.ResourceManager.GetString(resourceKey, AppResources.Culture) ?? $"[{resourceKey}]";

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetCulture(CultureInfo culture)
        {
            AppResources.Culture = culture;

            // Notify all bindings to update
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
