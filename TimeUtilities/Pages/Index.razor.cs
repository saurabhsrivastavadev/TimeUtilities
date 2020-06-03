using System;
using System.Threading.Tasks;
using System.Timers;

namespace TimeUtilities.Pages
{
    public partial class Index
    {
        // Constants 
        private readonly DateTime unixEpoch = DateTime.UnixEpoch;
        private readonly DateTime gpsEpoch = DateTime.Parse("6 January 1980");
        private readonly long gpsUtcEpochDeltaMillis;
        private readonly TimeZoneInfo tzInfo = TimeZoneInfo.Local;
        private readonly Timer timer = new Timer(100);

        // Variables
        private DateTime utcNow;
        private DateTime gpsNow;
        private long utcNowMillis;
        private long gpsNowMillis;

        public Index()
        {
            gpsUtcEpochDeltaMillis = (long)gpsEpoch.Subtract(unixEpoch).TotalMilliseconds;

            timer.Elapsed += async (sender, e) => await TimerTick();
            timer.Start();

            Console.WriteLine("Index page constructor !");
        }

        private Task TimerTick()
        {
            // populate instance variables
            utcNow = DateTime.UtcNow;
            gpsNow = utcNow.Subtract(TimeSpan.FromMilliseconds(gpsUtcEpochDeltaMillis));
            utcNowMillis = (long)utcNow.Subtract(unixEpoch).TotalMilliseconds;
            gpsNowMillis = (long)gpsNow.Subtract(gpsEpoch).TotalMilliseconds;

            // update the UI
            this.StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
