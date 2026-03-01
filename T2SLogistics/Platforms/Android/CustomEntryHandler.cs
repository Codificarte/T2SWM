using Android.Widget;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Platforms.Android
{
    public class CustomEntryHandler : EntryHandler
    {
        public CustomEntryHandler() : base(Mapper)
        {
        }
        public static IPropertyMapper<Entry, EntryHandler> Mapper = new PropertyMapper<Entry, EntryHandler>(EntryHandler.Mapper)
        {
            [nameof(Entry)] = (handler, view) =>
            {
                if (handler.PlatformView is EditText editText)
                {
                    editText.Background = null; // Remove underline
                    editText.SetPadding(0, 0, 0, 0); // Optional
                }
            }
        };
    }
}
