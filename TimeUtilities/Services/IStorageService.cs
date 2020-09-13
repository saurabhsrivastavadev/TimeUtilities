using BlazorUtils.JsInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeUtilities.Services
{
    internal interface IStorageService
    {
        public Task SaveTrackedTimezones(ISet<string> timezoneIds);
        public Task<ISet<string>> GetTrackedTimezones();
        public Task DeleteAllTrackedTimezones();

        delegate void TrackedTimezoneListUpdateCallbackType();
        TrackedTimezoneListUpdateCallbackType TrackedTimezoneListUpdateCallback { get; set; }
    }
}
