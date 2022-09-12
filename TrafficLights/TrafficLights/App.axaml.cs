using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TrafficLights.Models;
using TrafficLights.ViewModels;
using TrafficLights.Views;

namespace TrafficLights
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var model = new TrafficLightsModel();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(model)
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
