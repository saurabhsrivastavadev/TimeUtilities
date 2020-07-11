using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeUtilities.Shared
{
    public partial class TimezoneCard
    {
        [Inject]
        private ILogger<TimezoneCard> Logger { get; set; }

        [Parameter]
        public EventCallback OnCloseButtonClicked { get; set; }

        [Parameter]
        public string TimeZoneId
        {
            get
            {
                return _timeZoneId;
            }

            set
            {
                _timeZoneId = value;

                try
                {
                    _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
                }
                catch
                {
                    Logger.LogError($"Invalid timezone id {_timeZoneId}");
                }
            }
        }

        private string _timeZoneId;
        private TimeZoneInfo _timeZoneInfo;

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
                    string sign = _localTzOffset < 0 ? "-" : "+";
                    return hours > 0 ?
                        $"UTC ({sign}) {hours} hours and {minutes} minutes" :
                        $"UTC ({sign}) {minutes} minutes";
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Set the local tz name and offset
            if (_timeZoneInfo == null)
            {
                Logger.LogError("No timezone info set.");
                return;
            }

            _localTzOffset = (int)_timeZoneInfo.BaseUtcOffset.TotalMinutes;
            _localTzName = _timeZoneInfo.DisplayName;

            _localNow = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_localTzOffset));
        }

        // Refresh the time fields in the card
        // We don't run a refresh timer within this component
        //  since there would be multiple instances of this component.
        // Better to leave the refresh on to the page hosting this component/s.
        public void RefreshUI()
        {
            _localNow = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_localTzOffset));

            // update the UI
            this.StateHasChanged();
        }
    }
}
