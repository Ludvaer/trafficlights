using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using ReactiveUI;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.Logging;
using Splat;
using System.IO;
using TrafficLights.Interfaces;

namespace TrafficLights
{
    internal class Program
    {
        /// <summary>
        /// Инжектор зависимостей
        /// </summary>
        public static ServiceProvider Di { get; set; }

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // Настройка инжектора зависимостей
            Di = ConfigureServices()
                .BuildServiceProvider();

            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Setting up DI
        public static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ITrafficLights, Implementations.TrafficLights>();

            return services;
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
