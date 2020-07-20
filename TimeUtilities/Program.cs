using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeUtilities.Services;
using TimeUtilities.Services.Implementation;
using BlazorUtils.JsInterop;

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

            // Run the app
            await builder.Build().RunAsync();
        }
    }
}
