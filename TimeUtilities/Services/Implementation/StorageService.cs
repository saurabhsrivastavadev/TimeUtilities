using BlazorUtils.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TimeUtilities.Services.Implementation
{
    internal class StorageService : IStorageService
    {
        private const string TrackedTimezonesKey = "trackedTimezones";

        private ILogger Logger { get; set; }
        private IJsInteropService JSR { get; set; }

        public StorageService(ILogger<StorageService> logger, IJsInteropService jsInteropService)
        {
            Logger = logger;
            JSR = jsInteropService;
        }

        public async Task SaveTrackedTimezones(ISet<string> timezoneIds)
        {
            if (timezoneIds == null || timezoneIds.Count == 0)
            {
                Logger.LogError("No timezones to save.");
                return;
            }

            try
            {
                await JSR.StorageUtils.LocalStorageSetItem(
                    TrackedTimezonesKey, JsonSerializer.Serialize(timezoneIds));
            }
            catch
            {
                Logger.LogError("Failed to save to local storage.");
            }
        }

        public async Task<ISet<string>> GetTrackedTimezones()
        {
            try
            {
                string trackedTimezonesJson =
                    await JSR.StorageUtils.LocalStorageGetItem(TrackedTimezonesKey);
                return JsonSerializer.Deserialize<ISet<string>>(trackedTimezonesJson);
            }
            catch
            {
                Logger.LogError("Failed to get from local storage.");
            }

            return null;
        }

        public async Task DeleteAllTrackedTimezones()
        {
            try
            {
                await JSR.StorageUtils.LocalStorageDeleteItem(TrackedTimezonesKey);
            }
            catch
            {
                Logger.LogError("Failed to delete from local storage.");
            }
        }
    }
}
