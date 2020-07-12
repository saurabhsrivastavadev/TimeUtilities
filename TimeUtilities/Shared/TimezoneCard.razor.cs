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
        public EventCallback<string> OnCloseButtonClicked { get; set; }

        [Parameter]
        public IList<string> TimeZoneIdList { get; set; }

        [Parameter]
        public string TimeZoneId { get; set; }

        private IList<TimeZoneInfo> _timeZoneInfoList = new List<TimeZoneInfo>();

        protected override Task OnParametersSetAsync()
        {
            PopulateTimeZoneInfoList();

            // Set the local tz name and offset
            if (_timeZoneInfoList.Count == 0)
            {
                Logger.LogError("No timezone info set.");
            }

            return base.OnParametersSetAsync();
        }

        private DateTime GetTimeNowForTimeZone(TimeZoneInfo tzInfo)
        {
            return DateTime.UtcNow.Add(TimeSpan.FromMinutes(
                    (int)tzInfo.BaseUtcOffset.TotalMinutes));
        }

        private string GetTimeZoneOffsetString(int timeZoneOffset)
        {
            if (timeZoneOffset == -1)
            {
                return "NA";
            }
            else
            {
                int hours = Math.Abs(timeZoneOffset) / 60;
                int minutes = Math.Abs(timeZoneOffset) % 60;
                string sign = timeZoneOffset < 0 ? "-" : "+";
                return hours > 0 ?
                    $"UTC ({sign}) {hours} hours and {minutes} minutes" :
                    $"UTC ({sign}) {minutes} minutes";
            }
        }

        private void PopulateTimeZoneInfoList()
        {
            _timeZoneInfoList.Clear();

            if (!string.IsNullOrEmpty(TimeZoneId) &&
                TimeZoneIdList != null && TimeZoneIdList.Count > 0)
            {
                Logger.LogError(
                    "Both TimeZoneId and TimeZoneIdList parameters can't be set in TimezoneCard");
                return;
            }

            // Both TimeZoneId and TimeZoneIdList parameters can't be set
            if (!string.IsNullOrEmpty(TimeZoneId))
            {
                try
                {
                    _timeZoneInfoList.Add(TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
                }
                catch
                {
                    Logger.LogError($"Invalid timezone id {TimeZoneId}");
                }
            }
            else
            {
                foreach (var tzId in TimeZoneIdList)
                {
                    try
                    {
                        _timeZoneInfoList.Add(TimeZoneInfo.FindSystemTimeZoneById(tzId));
                    }
                    catch
                    {
                        Logger.LogError($"Invalid timezone id {tzId}");
                    }
                }
            }
        }

        private void HandleCloseButtonClicked(TimeZoneInfo tzInfo)
        {
            _timeZoneInfoList.Remove(tzInfo);

            if (OnCloseButtonClicked.HasDelegate)
            {
                OnCloseButtonClicked.InvokeAsync(tzInfo.DisplayName);
            }
        }
    }
}
