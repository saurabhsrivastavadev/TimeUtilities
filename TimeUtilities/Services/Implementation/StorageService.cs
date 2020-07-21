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

            await JSR.StorageUtils.LocalStorageSetItem(
                "trackedTimezones", JsonSerializer.Serialize(timezoneIds));
        }

        public async Task<ISet<string>> GetTrackedTimezones()
        {
            string trackedTimezonesJson =
                await JSR.StorageUtils.LocalStorageGetItem("trackedTimezones");

            return JsonSerializer.Deserialize<ISet<string>>(trackedTimezonesJson);
        }
    }
}
