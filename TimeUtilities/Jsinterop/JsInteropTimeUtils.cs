using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeUtilities.Jsinterop
{
    internal class JsInteropTimeUtils
    {
        // Singleton Instance
        public static JsInteropTimeUtils Instance { get; private set; }
        private JsInteropTimeUtils() { }

        // Javascript runtime
        // Must be set before using this class
        private static IJSRuntime _jsr;
        public static IJSRuntime JSR
        {
            private get
            {
                return _jsr;
            }

            set
            {
                _jsr = value;
                if (Instance == null)
                {
                    Instance = new JsInteropTimeUtils();
                }
            }
        }

        // Returns the offset from UTC in minutes
        public async Task<int> GetLocalTimezoneOffset()
        {
            return await JSR.InvokeAsync<int>("getLocalTimezoneOffset");
        }

        public async Task<string> GetLocalTimezoneName()
        {
            return await JSR.InvokeAsync<string>("getLocalTimezoneName");
        }

        public async Task<bool> ShowPwaInstallPrompt()
        {
            return await JSR.InvokeAsync<bool>("showPwaInstallPrompt");
        }

        public async Task<bool> IsPwaInstalled()
        {
            return await JSR.InvokeAsync<bool>("isPwaInstalled");
        }
    }
}
