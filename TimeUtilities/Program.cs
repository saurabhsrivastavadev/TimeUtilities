using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TimeUtilities.Services;
using TimeUtilities.Services.Implementation;
using BlazorUtils.JsInterop;
using BlazorUtils.Firebase;

namespace TimeUtilities
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Main() Entry");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Add root component
            builder.RootComponents.Add<App>("app");

            // Add services
            builder.Services.AddTransient(sp => new HttpClient {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            // Jsinterop service
            builder.Services.AddSingleton<IJsInteropService, JsInteropService>();

            // Storage service
            builder.Services.AddSingleton<IStorageService, StorageService>();

            // Firebase auth
            builder.Services.AddSingleton<IFirebaseGoogleAuthService, FirebaseGoogleAuthService>();

            // Run the app
            await builder.Build().RunAsync();
        }
    }
}
