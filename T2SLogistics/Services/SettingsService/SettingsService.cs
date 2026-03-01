using T2SLogistics.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.SettingsService
{
    public class SettingsService : ISettingsService
    {
        public const string ApplanguageKey = "ApplanguageKey";
        public const string AuthTokenKey = "AuthTokenKey";
        public const string BaseUrlKey = "BaseUrlKey";
        public const string UsernameKey = "UsernameKey";
        public const string EmailKey = "EmailKey";
        public const string UserIdKey = "UserIdKey";

        public const string UserCodeKey = "UserCodeKey";
        public string Applanguage
        {
            get => Preferences.Default.Get(ApplanguageKey, string.Empty);
            set => AddOrUpdateValue(ApplanguageKey, value);
        }
        public string AuthToken
        {
            get => Preferences.Default.Get(AuthTokenKey, string.Empty);
            set => AddOrUpdateValue(AuthTokenKey, value);
        }
        public string BaseUrl
        {
            get => Preferences.Default.Get(BaseUrlKey, string.Empty);
            set => AddOrUpdateValue(BaseUrlKey, value);
        }
        public string Username 
        {
            get => Preferences.Default.Get(UsernameKey, string.Empty);
            set => AddOrUpdateValue(UsernameKey, value);
        }
        public string Email
        {
            get => Preferences.Default.Get(EmailKey, string.Empty);
            set => AddOrUpdateValue(EmailKey, value);
        }
        public string UserCode
        {
            get => Preferences.Default.Get(UserCodeKey, string.Empty);
            set => AddOrUpdateValue(UserCodeKey, value);
        }
        public string UserId
        {
            get => Preferences.Default.Get(UserIdKey, string.Empty);
            set => AddOrUpdateValue(UserIdKey, value);
        }

        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                if (Preferences.Default.ContainsKey(key))
                {
                    Preferences.Default.Remove(key);
                }
            }

            try
            {
                Preferences.Default.Set(key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        public void ClearAllSettings()
        {
            Preferences.Default.Clear();
        }
    }
}
