using CommonDateTimeUtils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TimeUtilities.Jsinterop;

namespace TimeUtilities.Pages
{
    public partial class Index
    {
        // Framework Injections
        [Inject]
        private ILogger<Index> Logger { get; set; }

        [Inject]
        private IJSRuntime JSR 
        { 
            set
            {
                JsInteropTimeUtils.JSR = value;
            }
        }

        private readonly System.Timers.Timer _uiRefreshTimer = new System.Timers.Timer(200);

        // Fields
        private DateTime _utcNow;
        private DateTime _localNow;
        private int _localTzOffset = -1;
        private string _localTzName = "";

        private string LocalTzOffsetStr
        {
            get
            {
                if (_localTzOffset == -1)
                {
                    return "NA";
                }
                else
                {
                    int hours = Math.Abs(_localTzOffset) / 60;
                    int minutes = Math.Abs(_localTzOffset) % 60;
                    // sign is reversed since we need to display with UTC
                    string sign = _localTzOffset < 0 ? "+" : "-";
                    return hours > 0 ?
                        $"UTC ({sign}) {hours} hours and {minutes} minutes" :
                        $"UTC ({sign}) {minutes} minutes";
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("OnInitializedAsync()");

            _uiRefreshTimer.Elapsed += TimerTick;
            _uiRefreshTimer.Start();

            _localTzOffset = await JsInteropTimeUtils.Instance?.GetLocalTimezoneOffset();
            _localTzName = await JsInteropTimeUtils.Instance?.GetLocalTimezoneName();

            await base.OnInitializedAsync();
        }

        private void TimerTick(object sender, ElapsedEventArgs args)
        {
            // populate instance variables
            _utcNow = DateTime.UtcNow;
            _localNow = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(_localTzOffset));

            // update the UI
            this.StateHasChanged();
        }
    }
}
